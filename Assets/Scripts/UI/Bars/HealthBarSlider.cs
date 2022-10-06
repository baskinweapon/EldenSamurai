

namespace UI.Bars {
    public class HealthBarSlider : BarSlider {
        public Health health;
        
        protected override void SetValues() {
            base.SetValues();
            currentValuePercent = health.GetCurrentHPPercent();
            maxValue = health.GetMaxHealth();
            persistantValue = health.GetPersistantValue();
        }
    }
}