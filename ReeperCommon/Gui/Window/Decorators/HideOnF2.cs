namespace ReeperCommon.Gui.Window.Decorators
{
    public class HideOnF2 : WindowDecorator
    {
        private bool _interfaceVisible = false;
        private bool _restoreVisibility = false; // because we don't want to accidentally show the window if it was
                                                 // hidden when F2 was pressed (unless the caller explicitly makes it
                                                 // visible during this time)



        public HideOnF2(IWindowComponent baseComponent) : base(baseComponent)
        {
            GameEvents.onShowUI.Add(Show);
            GameEvents.onHideUI.Add(Hide);
        }



        ~HideOnF2()
        {
            GameEvents.onHideUI.Remove(Hide);
            GameEvents.onShowUI.Remove(Show);
        }



        private void Show()
        {
            _interfaceVisible = true;
            Visible = true;
        }



        private void Hide()
        {
            _interfaceVisible = false;

            base.Visible = false;
        }



        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = _restoreVisibility && _interfaceVisible; // if UI not visible, 
                            // don't allow window to be explicitly made visible (although the change is 
                            // cached so it will appear when UI is shown again)

                _restoreVisibility = value;
            }
        }
    }
}
