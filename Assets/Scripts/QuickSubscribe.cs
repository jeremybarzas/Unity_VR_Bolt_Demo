using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSubscribe : MonoBehaviour
{
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    [SerializeField]
    private SubscriptionEvent[] m_subscriptionEvents;
       
	void Awake ()
    {
        for(int i = 0; i < m_subscriptionEvents.Length; i++)
        {
            m_subscriptionEvents[i].m_subCallback = m_subscriptionEvents[i].InvokeUnityEvent;
            m_subscriptions.Subscribe(m_subscriptionEvents[i].m_subMessage, m_subscriptionEvents[i].m_subCallback);
        }
	}

    public void NotifySubscribers(string message)
    {
        print(message);
        Mouledoux.Components.Mediator.instance.NotifySubscribers(message);
    }

    void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }


    [System.Serializable]
    internal struct SubscriptionEvent
    {
        public string m_subMessage;
        public UnityEngine.Events.UnityEvent m_event;

        public Mouledoux.Callback.Callback m_subCallback;
        
        public void InvokeUnityEvent(object[] emptyPacket)
        { m_event.Invoke(); }
    }
}