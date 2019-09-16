namespace Mouledoux.Callback
{
    /// <summary>
    /// Callback action to be used by all subscribers
    /// </summary>
    /// <param name="data">Predefined data Packet to act as potential arguments for subscriptions</param>
    //public delegate void Callback(Packet data);

    public delegate void Callback(object[] args);

    /// <summary>
    /// Collecion of basic variables to be sent via delegates
    /// </summary>
    [System.Serializable]
    public sealed class Packet
    {
        /// <summary>
        /// All of the intigers to be used
        /// </summary>
        public int[] ints;
        /// <summary>
        /// All of the boolens to be used
        /// </summary>
        public bool[] bools;
        /// <summary>
        /// All of the floating point numbers to be used
        /// </summary>
        public float[] floats;
        /// <summary>
        /// All of the text strings to be used
        /// </summary>
        public string[] strings;

        /// <summary>
        /// Default constructor
        /// To be used to send empty packets
        /// </summary>
        public Packet()
        {
            this.ints = new int[0];
            this.bools = new bool[0];
            this.floats = new float[0];
            this.strings = new string[0];
        }

        /// <summary>
        /// Constructor to ensure all arrays are set
        /// </summary>
        /// <param name="ints">Predefined array of ints</param>
        /// <param name="bools">Predefined array of bools</param>
        /// <param name="floats">Predefined array of floats</param>
        /// <param name="strings">Predefined array of strings</param>
        public Packet(int[] ints, bool[] bools, float[] floats, string[] strings)
        {
            this.ints = ints;
            this.bools = bools;
            this.floats = floats;
            this.strings = strings;
        }
    }
}