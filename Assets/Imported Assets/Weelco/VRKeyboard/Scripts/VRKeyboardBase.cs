namespace Weelco.VRKeyboard {

    public class VRKeyboardBase : VRKeyboardData {

        public delegate void VRKeyboardBtnClick(string value);
        public VRKeyboardBtnClick OnVRKeyboardBtnClick;

        protected bool Initialized = false;
    }
}
