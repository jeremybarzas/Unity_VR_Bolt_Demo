using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtils : MonoBehaviour
{
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void PushUIButton(Button b){
        b.onClick.Invoke();
    }
}
