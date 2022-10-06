namespace UI.Bars {
    public class ExpirienceBarSlider : BarSlider {
        public Expirience expirience;

        protected override void SetValues() {
            base.SetValues();
            currentValuePercent = expirience.GetCurrentExpProcent();
            maxValue = expirience.GetNeddedExpirience();
        }
    }
}