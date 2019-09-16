namespace Weelco.VRKeyboard {

    public class VRKeyboardLite : VRKeyboardBase {
        
        private VRKeyboardButton[] keys;

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
                    keys[i].SetKeyText(liteSpecials[i]);
                    keys[i].OnVRKeyboardBtnClick += HandleClick;
                }

                Initialized = true;
            }
        }
        
        private void HandleClick(string value) {
            if (OnVRKeyboardBtnClick != null)
                OnVRKeyboardBtnClick(value);            
        }
    }
}