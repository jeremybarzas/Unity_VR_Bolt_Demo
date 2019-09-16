using System.Collections.Generic;
using UnityEngine;

namespace Weelco.VRKeyboard {

    public class VRKeyboardExtra : VRKeyboardBase {
        
        private GameObject keysLetters;
        private GameObject keysSpecials;
        private IDictionary<GameObject, VRKeyboardButton[]> keysDict;

        private bool areLettersActive = false;
        private bool isLowercase = false;
        
        void OnDestroy() {
            if (Initialized) {
                foreach (KeyValuePair<GameObject, VRKeyboardButton[]> entry in keysDict) {
                    var value = entry.Value;
                    for (int i = 0; i < value.Length; i++) {
                        value[i].OnVRKeyboardBtnClick -= HandleClick;
                    }
                }
            }
        }
        
        public void Init() {
            if (!Initialized) {
                keysLetters = transform.Find("KeysLetters").gameObject;
                keysSpecials = transform.Find("KeysSpecials").gameObject;
                keysDict = new Dictionary<GameObject, VRKeyboardButton[]>();

                keysDict.Add(keysLetters, keysLetters.GetComponentsInChildren<VRKeyboardButton>());
                keysDict.Add(keysSpecials, keysSpecials.GetComponentsInChildren<VRKeyboardButton>());

                foreach (KeyValuePair<GameObject, VRKeyboardButton[]> entry in keysDict) {
                    var value = entry.Value;
                    for (int i = 0; i < value.Length; i++) {
                        value[i].Init();
                        value[i].OnVRKeyboardBtnClick += HandleClick;
                    }
                }

                ChangeSpecialLetters();
                LowerUpperKeys();

                Initialized = true;
            }
        }

        private void HandleClick(string value) {
            if (value.Equals(NUM) || value.Equals(ABC)) {
                ChangeSpecialLetters();
            }
            else if (value.Equals(UP) || value.Equals(LOW)) {
                LowerUpperKeys();
            }
            else {
                if (OnVRKeyboardBtnClick != null)
                    OnVRKeyboardBtnClick(value);
            }
        }

        private void ChangeSpecialLetters() {
            areLettersActive = !areLettersActive;
            keysLetters.SetActive(areLettersActive);
            keysSpecials.SetActive(!areLettersActive);            
            string[] ToDisplay = areLettersActive ? (isLowercase ? extraLettersLowercase : extraLettersUppercase) : extraSpecials;
            VRKeyboardButton[] keys = areLettersActive ? keysDict[keysLetters] : keysDict[keysSpecials];
            for (int i = 0; i < keys.Length; i++) {
                keys[i].SetKeyText(ToDisplay[i]);
            }
        }

        private void LowerUpperKeys() {
            isLowercase = !isLowercase;
            string[] ToDisplay = isLowercase ? extraLettersLowercase : extraLettersUppercase;
            VRKeyboardButton[] keys = keysDict[keysLetters];
            for (int i = 0; i < keys.Length; i++) {
                keys[i].SetKeyText(ToDisplay[i]);
            }
        }
    }
}