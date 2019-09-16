using UnityEngine;

namespace Weelco.VRKeyboard {
    
    public class VRKeyboardFull : VRKeyboardBase {
        
        private VRKeyboardButton[] keys;
        private bool areLettersActive = true;
        private bool isLowercase = true;        
        
        void OnDestroy() {
            if (Initialized) {
                foreach (VRKeyboardButton key in keys) {
                    key.OnVRKeyboardBtnClick -= HandleClick;
                }
            }
        }

        public void Init() {
            if (!Initialized) {
                keys = transform.GetComponentsInChildren<VRKeyboardButton>();
                for (int i = 0; i < keys.Length; i++) {
                    keys[i].Init();
                    keys[i].SetKeyText(allLettersLowercase[i]);
                    keys[i].OnVRKeyboardBtnClick += HandleClick;
                }

                Initialized = true;
            }
        }

        private void HandleClick(string value) {
            Debug.Log("Handle Click : " + value);
            if (value.Equals(SYM) || value.Equals(ABC)) {
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
            string[] ToDisplay = areLettersActive ? (isLowercase ? allLettersLowercase : allLettersUppercase) : allSpecials;
            bool ignoreIcon = false;
            for (int i = 0; i < keys.Length; i++) {
                ignoreIcon = (!areLettersActive && (ToDisplay[i].Equals("№")));
                keys[i].SetKeyText(ToDisplay[i], ignoreIcon);
            }
        }

        private void LowerUpperKeys() {
            isLowercase = !isLowercase;
            string[] ToDisplay = isLowercase ? allLettersLowercase : allLettersUppercase;
            for (int i = 0; i < keys.Length; i++) {
                keys[i].SetKeyText(ToDisplay[i]);
            }
        }        
    }
}