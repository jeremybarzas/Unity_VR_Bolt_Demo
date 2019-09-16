using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeyboardKey : InteractableObject
{
    [SerializeField]
    private string m_keyboardID;

    [SerializeField] private string m_mainKey;
    [SerializeField] private string m_shiftKey;

    [Space]
    public bool isShifted;
    public bool isEnabled;

    [Header("TextMesh")]
    [SerializeField] private TMPro.TextMeshPro m_mainText;
    [SerializeField] private TMPro.TextMeshPro m_shiftText;

    [Header("Materials")]
    [SerializeField] private Sprite m_normalMat;
    [SerializeField] private Sprite m_highlightMat;
    [SerializeField] private Sprite m_pressedMat;
    [SerializeField] private Sprite m_disabledMat;
    [SerializeField] private SpriteRenderer m_backgroundMesh;

    [SerializeField] private AudioSource m_clickNoise;
    [SerializeField] private AudioSource m_hoverNoise;

    private Mouledoux.Components.Mediator.Subscriptions m_subscriptions = 
        new Mouledoux.Components.Mediator.Subscriptions();

    private Mouledoux.Callback.Callback onShift, onKeyPress;

    private bool m_alreadySent = false;

    public void Initialize(string a_keyboardID, string a_mainString, string a_shiftString = "", bool a_capsShift = true)
    {
        m_keyboardID = a_keyboardID;
        m_mainKey = a_mainString.ToLower();
        //m_shiftKey = a_shiftString == "" ? m_mainKey.ToUpper() : a_shiftString;

        m_onHighnight.AddListener(delegate{SetMaterial(m_highlightMat); m_alreadySent = false;
                                    m_hoverNoise.Play();});
        m_offHighnight.AddListener(delegate{SetMaterial(m_normalMat); m_alreadySent = false;});
        m_onInteract.AddListener(delegate{PressKey();});
        m_offInteract.AddListener(delegate{m_alreadySent = false;});

        onShift = null;
        onKeyPress = null;
        m_subscriptions.UnsubscribeAll();

        onKeyPress += ShiftKey;

        m_subscriptions.Subscribe($"vrkeyboard:{m_keyboardID}", onKeyPress);

        DefaultKey();
        ResetMaterial();
    }


    public new void Start()
    {
        base.Start();
        Initialize(m_keyboardID, m_mainKey);
    }

    public void PressKey()
    {
        if(m_alreadySent == false){
            Mouledoux.Components.Mediator.instance.NotifySubscribers($"vrkeyboard:{m_keyboardID}", new object[] {this});
            m_clickNoise.Play();
            m_alreadySent = true;
        } 
    }


    public string GetCurrentKey()
    {
        return isShifted ? GetShiftKey() : GetMainKey();
    }

    public string GetMainKey()
    {
        return m_mainKey;
    }

    public string GetShiftKey()
    {
        return m_shiftKey;
    }



    public int DefaultKey()
    {
        isShifted = false;
        m_mainText.text = m_mainKey;

        if(m_shiftText != null)
            m_shiftText.text = m_shiftKey;

        return 0;
    }

    public int ShiftKey()
    {
        if(isShifted)
        {
            DefaultKey();
            return 1;
        }

        isShifted = true;
        m_mainText.text = m_shiftKey;

        if(m_shiftText != null)
            m_shiftText.text = m_mainKey;

        return 0;
    }

    public void ShiftKey(object[] args)
    {
        if(isShifted)
        {
            DefaultKey();
        }

        else if(args[0].GetType() == typeof(VRKeyboardKey))
        {
            VRKeyboardKey key = (VRKeyboardKey)args[0];
            if(key.GetMainKey() == "shift")
            {
                ShiftKey();
            }
        }
    }


    public bool SetKeyEnabled(bool a_enabled)
    {
        isEnabled = a_enabled;
        SetMaterial(isEnabled ? m_normalMat : m_disabledMat);
        return isEnabled;
    }

    private void SetMaterial(Sprite mat)
    {
        m_backgroundMesh.sprite = mat;
    }
    private void ResetMaterial()
    {
        SetMaterial(m_normalMat);
    }

    [ContextMenu("On Interact")]
    private void OnInteract(){
        m_onInteract.Invoke();
    }

    [ContextMenu("Off Interact")]
    private void OffInteract(){
        m_offInteract.Invoke();
    }

    [ContextMenu("On Highlight")]
    private void OnHighlight(){
        m_onHighnight.Invoke();
    }

    [ContextMenu("Off Highlight")]
    private void OffHighlight(){
        m_offHighnight.Invoke();
    }

    [ContextMenu("Press Key")]
    private void PressKeyInspector(){
        PressKey();
    }

    private void OnDestroy() {
        m_subscriptions.UnsubscribeAll();
    }
}