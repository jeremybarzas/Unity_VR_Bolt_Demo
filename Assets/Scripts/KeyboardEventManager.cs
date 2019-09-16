using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardEventManager : MonoBehaviour
{
    #region -----SINGLETON-----
    private static KeyboardEventManager _instance;

    public static KeyboardEventManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<KeyboardEventManager>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (instance != this) Destroy(this);

        //DontDestroyOnLoad(instance);
    }
    #endregion

    [SerializeField]
    private List<KeyboardEvent> m_keyEvents = new List<KeyboardEvent>();


    void Update ()
    {
		foreach(KeyboardEvent key in m_keyEvents)
        {
            if (Input.GetKey(key.m_key) && key.m_hold) key.m_event.Invoke();

            else if (Input.GetKeyDown(key.m_key)) key.m_event.Invoke();
        }
	}

    public void TriggerEvent(string eventName)
    {
        foreach (KeyboardEvent key in m_keyEvents)
        {
            if (eventName.ToLower() == key.m_name.ToLower())
            {
                key.m_event.Invoke();
                return;
            }
        }
    }

    public void Broadcast(string message)
    {
        Mouledoux.Components.Mediator.instance.NotifySubscribers(message, new object[]{});
    }
}

[System.Serializable]
public sealed class KeyboardEvent
{
    public string m_name;

    public KeyCode m_key;
    public bool m_hold;
    public UnityEngine.Events.UnityEvent m_event;
}