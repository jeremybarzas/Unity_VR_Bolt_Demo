using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeyboard : MonoBehaviour
{
    private char[] qwerty =
    {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
    'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
    'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';',
    'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/'};

    private char[] qwertySHIFT =
    {'!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
    'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
    'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':',
    'Z', 'X', 'C', 'V', 'B', 'N', 'M', '<', '>', '?'};


    public string m_keyboardID = "0000";
    [SerializeField] public GameObject KeyPrefab;
    private List<VRKeyboardKey> m_keys = new List<VRKeyboardKey>();

    // Start is called before the first frame update
    void Start()
    {
        if(m_keys.Count == 0)
            GenerateNewQWERTY();
    }

    [ContextMenu("Shift")]
    public void Shift()
    {
        foreach(VRKeyboardKey key in m_keys)
        {
            key.ShiftKey();
        }
    }


    public void GenerateNewQWERTY()
    {
        while(m_keys.Count > 0)
        {
            Destroy(m_keys[0]);
            m_keys.RemoveAt(0);
        }
        m_keys.Clear();

        GameObject newKey;
        for(int i = 0; i < qwerty.Length; i++)
        {
            newKey = Instantiate<GameObject>(KeyPrefab, (transform.position + transform.right * 4.5f) + transform.right * -(i%10), Quaternion.identity);
            newKey.transform.parent = transform;
            newKey.transform.Translate((Vector3.up * 1.5f) + Vector3.down * (i/10));

            m_keys.Add(newKey.GetComponent<VRKeyboardKey>());
            m_keys[i].Initialize(m_keyboardID, qwerty[i].ToString(), qwertySHIFT[i].ToString());
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
