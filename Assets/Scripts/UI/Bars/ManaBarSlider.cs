namespace UI.Bars {
    public class ManaBarSlider : BarSlider {
        public Mana mana;

        protected override void SetValues() {
            base.SetValues();
            currentValuePercent = mana.GetCurrentManaPercant();
            maxValue = mana.GetMaxMana();
            persistantValue = mana.GetPersistantValue();
        }
    }
}