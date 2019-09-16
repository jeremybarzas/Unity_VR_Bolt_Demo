namespace Mouledoux.Components
{
    /// <summary>
    /// Static class for all mediation.
    /// </summary>
    public sealed class Mediator
    {
        /// The below code is a standard singleton
        #region Singleton
        private Mediator() { }

        private static Mediator _instance;

        public static Mediator instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Mediator();
                }

                return _instance;
            }
        }
        #endregion Singleton



        /// <summary>
        /// Dictionary of subscription strings and associated delegate callbacks
        /// </summary>
        private System.Collections.Generic.Dictionary<string, Callback.Callback> subscriptions =
            new System.Collections.Generic.Dictionary<string, Callback.Callback>();

        /// <summary>
        /// Dictionary of blocked messages, and their expiration time
        /// </summary>
        private System.Collections.Generic.Dictionary<string, BlockedMessage> blockedMessages =
            new System.Collections.Generic.Dictionary<string, BlockedMessage>();


        private System.Collections.Generic.List<string> heldMessages = 
        new System.Collections.Generic.List<string>();





        /// <summary>
        /// Checks to see if their are any Subscribers to the broadcasted message
        /// and invokes ALL callbacks associated with it
        /// </summary>
        /// 
        /// <param name="message">The message to be broadcasted (case sensitive)</param>
        /// <param name="data">Packet of information to be used by ALL receiving parties</param>    
        /// 
        /// <returns>
        /// 0 the message was broadcasted successfully
        /// 1 the there were no subscribers to the message
        /// -1 the message is blocked
        /// </returns>
        public int NotifySubscribers(string message, object[] args = null, bool holdMessage = false)
        {
            message = message.ToLower();

            // Temporary BlockedMessage for checking blacklist
            BlockedMessage blocked;

            // Checks if the message is being blocked, and reduces the remaining time if it is
            if(blockedMessages.TryGetValue(message, out blocked))
                if (blocked.blockTime < 0 || --blocked.blockTime > 0) return -1;

            // Makes sure the datapack has been set to something, even if one isn't provided
            args = args == null ? new object[0] : args;

            // Temporary delegate container for modifying subscription delegates 
            Callback.Callback cb;

            // Check to see if the message has any valid subscriptions
            if (instance.subscriptions.TryGetValue(message, out cb))
            {
                // Invokes all associated delegates with the args array
                cb.Invoke(args);

                return 0;
            }

            else if(holdMessage && !heldMessages.Contains(message))
            {
                heldMessages.Add(message);
            }

            return 1;
        }





        /// <summary>
        /// Disables the message without unsubscribing
        /// </summary>
        /// 
        /// <param name="message">Message to be blocked</param>
        /// <param name="blockTime">How many times the message will be blocked before the block expires</param>
        /// <param name="additive">If the blockTime passed should be added to the remaining time, or replace the remaining time, if the message is already blocked</param>
        /// 
        /// <returns>
        /// 0 the message has been blocked
        /// 1 the message is already blocked
        /// </returns>
        public int BlockMessage(string message, int blockTime, bool additive = false)
        {
            message = message.ToLower();

            if (blockedMessages.ContainsKey(message))
            {
                blockTime += additive ? blockedMessages[message].blockTime : 0;

                blockedMessages.Remove(message);
                BlockMessage(message, blockTime);

                return 1;
            }

            else
            {
                blockedMessages.Add(message, new BlockedMessage(message, blockTime));
                return 0;
            }
        }



        /// <summary>
        /// Unblocks a previously blocked message
        /// </summary>
        /// 
        /// <param name="message">Message to be unblocked</param>
        /// 
        /// <returns>
        /// 0 message was unblocked
        /// -1 message was not blocked to begin
        /// </returns>
        public int UnblockMessage(string message)
        {
            message = message.ToLower();

            if (!blockedMessages.ContainsKey(message)) return -1;
            
            blockedMessages.Remove(message);
            return 0;
        }









        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Struct for storing a blocked message, and expiration time
        /// </summary>
        private struct BlockedMessage
        {
            /// <summary>
            /// Constructor for blocked messages
            /// </summary>
            /// <param name="aMessage">Message to be blocked</param>
            /// <param name="aBlockTime">How many broadcast attempty will be blocked. Set to -1 for infinity</param>
            public BlockedMessage(string aMessage, int aBlockTime)
            {
                message = aMessage.ToLower();
                blockTime = aBlockTime;
            }


            /// <summary>
            /// Message to be blocked
            /// </summary>
            public string message;

            /// <summary>
            /// How many times the message will be rejected before the block expires
            /// Less than 0 requires manual unblocking
            /// </summary>
            public int blockTime;
        }









        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Base class for all entities that will be listing for broadcasts
        /// </summary>
        public sealed class Subscriptions
        {
            /// <summary>
            /// Personal, internal record of all active subscriptions
            /// </summary>
            private System.Collections.Generic.Dictionary<string, Callback.Callback> localSubscriptions =
                 new System.Collections.Generic.Dictionary<string, Callback.Callback>();


            /// <summary>
            /// Links a custom delegate to a message in a SPECIFIC subscription dictionary
            /// </summary>
            /// <param name="container">Reference to the dictionary of subscriptions we want to modify</param>
            /// <param name="message">The message to subscribe to (case sensitive)</param>
            /// <param name="callback">The delegate to be linked to the broadcast message</param>
            private void Subscribe(ref System.Collections.Generic.Dictionary<string, Callback.Callback> container, string message, Callback.Callback callback)
            {
                message = message.ToLower();

                // Temporary delegate container for modifying subscription delegates 
                Callback.Callback cb;

                // Check to see if there is not already a subscription to this message
                if (!container.TryGetValue(message, out cb))
                {
                    // If there is not, then make one with the message and currently empty callback delegate
                    container.Add(message, cb);
                }

                /// If the subscription does already exist,
                /// then cb is populated with all associated delegates,
                /// if it does not, cb is empty.

                // Add the delegate to cb (new or populated)
                cb += callback;
                // Set the delegate linked to the message to cb
                container[message] = cb;
            }


            /// <summary>
            /// Links a custom delegate to a message that may be breadcasted via a Publisher
            /// </summary>
            /// <param name="message">The message to subscribe to (case sensitive)</param>
            /// <param name="callback">The delegate to be linked to the broadcast message</param>
            public void Subscribe(string message, Callback.Callback callback, bool acceptStaleMessages = false)
            {
                // First, adds the subscription to the internal records
                Subscribe(ref localSubscriptions, message, callback);
                // Then, adds the subscription to the public records
                Subscribe(ref instance.subscriptions, message, callback);


                if(acceptStaleMessages && instance.heldMessages.Contains(message))
                {
                    instance.heldMessages.Remove(message);
                    callback.Invoke(null);
                }
            }


            /// <summary>
            /// Unlinks a custom delegate from a message in a SPECIFIC subscription dictionary
            /// </summary>
            /// <param name="container">Reference to the dictionary of subscriptions we want to modify</param>
            /// <param name="message">The message to unsubscribe from (case sensitive)</param>
            /// <param name="callback">The delegate to be removed from the broadcast message</param>
            private void Unsubscribe(ref System.Collections.Generic.Dictionary<string, Callback.Callback> container, string message, Callback.Callback callback)
            {
                message = message.ToLower();
                
                // Temporary delegate container for modifying subscription delegates 
                Callback.Callback cb;

                // Check to see if there is a subscription to this message
                if (container.TryGetValue(message, out cb))
                {
                    /// If the subscription does already exist,
                    /// then cb is populated with all associated delegates.
                    /// Otherwise nothing will happen

                    // Remove the selected delegate from the callback
                    cb -= callback;

                    // Check the modified cb to see if there are any delegates left
                    if (cb == null)
                    {
                        // If there is not, then remove the subscription completely
                        container.Remove(message);
                    }
                    else
                    {
                        // If there are some left, reset the callback to the now lesser cb
                        container[message] = cb;
                    }
                }
            }


            /// <summary>
            /// Unlinks a custom delegate from a message that may be breadcasted via a Publisher
            /// </summary>
            /// <param name="message">The message to unsubscribe from</param>
            /// <param name="callback">The delegate to be unlinked from the broadcast message</param>
            public void Unsubscribe(string message, Callback.Callback callback)
            {
                message = message.ToLower();

                // First, remove the subscription from the internal records
                Unsubscribe(ref localSubscriptions, message, callback);
                // Then, remove the subscription from the public records
                Unsubscribe(ref instance.subscriptions, message, callback);
            }


            /// <summary>
            /// Unlinks all (local) delegates from given broadcast message
            /// </summary>
            /// <param name="message">The message to unsubscribe from</param>
            public void UnsubscribeAllFrom(string message)
            {
                message = message.ToLower();

                Unsubscribe(message, localSubscriptions[message]);
            }


            /// !!! IMPORTANT !!! ///
            /// The method below - UnsubscribeAll()
            /// MUST BE CALLED whenever a class using a subscriber is removed
            /// If it is not, you WILL GET NULL REFERENCE ERRORS

            /// <summary>
            /// Unlinks all (local) delegates from every (local) broadcast message
            /// </summary>
            public void UnsubscribeAll()
            {
                foreach (string message in localSubscriptions.Keys)
                {
                    Unsubscribe(ref instance.subscriptions, message, localSubscriptions[message]);
                }

                localSubscriptions.Clear();
            }

            ~Subscriptions()
            {
                UnsubscribeAll();
            }
        }
    }
}
