using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractionType
    {
        DEFAULT,
        PICKUP,
        LONGINTERACT,
    }

    public InteractionType m_interactionType;
    public bool m_lockedInPlace = false;

    //public bool m_pickup;
    //[SerializeField]
    //private bool repickup;
    //[HideInInspector]
    //public bool m_repickup { get { return repickup; } }

    public UnityEngine.Events.UnityEvent m_onHighnight;
    public UnityEngine.Events.UnityEvent m_offHighnight;
    public UnityEngine.Events.UnityEvent m_onInteract;
    public UnityEngine.Events.UnityEvent m_offInteract;

    public Mouledoux.Callback.Packet m_IOPacket;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = new Mouledoux.Components.Mediator.Subscriptions();

    protected Mouledoux.Callback.Callback onHighlight = null;
    protected Mouledoux.Callback.Callback offHighlight = null;
    protected Mouledoux.Callback.Callback onInteract = null;
    protected Mouledoux.Callback.Callback offInteract = null;

    protected void Start()
    {
        Initialize(gameObject);
    }

    protected void Initialize(GameObject self)
    {
        onHighlight += OnHighlight;
        offHighlight += OffHighlight;

        onInteract += OnInteract;
        offInteract += OffInteract;

        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->onhighlight", onHighlight);
        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->offhighlight", offHighlight);

        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->oninteract", onInteract);
        m_subscriptions.Subscribe(self.GetInstanceID().ToString() + "->offinteract", offInteract);
    }

    void OnDestroy()
    {
        ClearSubscriptions();
    }

    public void ClearSubscriptions()
    {
        m_subscriptions.UnsubscribeAll();
    }

    protected void OnHighlight(object[] args)
    {
        m_onHighnight.Invoke();
    }

    protected void OffHighlight(object[] args)
    {
        m_offHighnight.Invoke();
    }


    protected void OnInteract(object[] args)
    {
        m_onInteract.Invoke();
    }

    [ContextMenu("Off Interact")]
    protected void OffInteract(object[] args)
    {
        m_offInteract.Invoke();
    }

    public void ToggleGameObject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}