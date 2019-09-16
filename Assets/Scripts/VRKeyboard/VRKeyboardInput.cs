using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.InputField))]
public class VRKeyboardInput : MonoBehaviour
{
    [SerializeField]
    private string m_keyboardID;
    public string keyboardID
    {
        get {return m_keyboardID;}

        set
        {
            m_keyboardID = value;
            m_subscriptions.UnsubscribeAll();

            AppendText = null;
            AppendText += AddText;
            m_subscriptions.Subscribe($"vrkeyboard:{m_keyboardID}", AppendText);
        }
    }

    private UnityEngine.UI.InputField m_inputField;
    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions =
        new Mouledoux.Components.Mediator.Subscriptions();
    private Mouledoux.Callback.Callback AppendText;

    private string email = "";

    private string password = "";

    private string first = "";

    private string last = "";

    // Start is called before the first frame update
    void Start()
    {
        keyboardID = m_keyboardID;
        m_inputField = GetComponent<UnityEngine.UI.InputField>();
    }

    void Update()
    {
        //m_inputField.text = OVRManager.display.acceleration.ToString();
    }

    public void AddText(object[] args)
    {
        if(args[0].GetType() == typeof(VRKeyboardKey))
        {
            VRKeyboardKey key = (VRKeyboardKey)args[0];

            if(key.GetMainKey() == ".com"){
                m_inputField.text += ".com";
                return;
            }         

            switch(key.GetMainKey().ToLower())
            {
                case "bks":
                    m_inputField.text = m_inputField.text.Remove(m_inputField.text.Length - 1);
                    break;

                case "space":
                    m_inputField.text += " ";
                    break;

                case "shift":
                    break;

                default:
                    m_inputField.text += key.GetCurrentKey();
                    break;
            }
        }
    }

    public void ChangeAllKeyID(string s){
        var keys = FindObjectsOfType<VRKeyboardKey>();

        foreach(VRKeyboardKey k in keys)
        {
            k.Initialize(s, k.GetMainKey());
        }
    }

    public void SendInformation(string s){
        if(s == null || s == "") return;

        Mouledoux.Components.Mediator.instance.NotifySubscribers(s, new object[] {m_inputField.textComponent});
    }

    private void OnDestroy()
    {
        m_subscriptions.UnsubscribeAll();
    }
}
