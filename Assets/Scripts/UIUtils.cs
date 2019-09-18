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

    public static Transform FindChild(string childname, Transform root){
        var openList = new List<Transform>();
        var closedList = new List<Transform>();

        openList.Add(root);

        while(openList.Count > 0){
            foreach(var t in openList){
                if(t.name == childname){
                    return t;
                }
            }

            var newOpens = new List<Transform>();
            foreach(Transform t in openList){
                foreach(Transform child in t){
                    newOpens.Add(child);
                }
            }

            foreach(Transform t in openList){
                closedList.Add(t);
            }

            openList = null;
            openList = newOpens;
        }

        return null;
    }
}
