using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities {
    public class AbilityCooldown : MonoBehaviour {
        public Image darkMask;
        public Image noManaMask;
        public TextMeshProUGUI coolDowntText;
        
        [SerializeField]
        private Ability ability;

        [SerializeField]
        private GameObject abilityHolder;

        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private AudioSource abilitiSource;
        
        private float coolDownDuration;
        private float nextReadyTime;
        private float coolDownTimeLeft;

        private void Start() {
            if (!ability) this.enabled = false;
            Initiallize(ability, abilityHolder);
        }
        
        public void Initiallize(Ability ability, GameObject _abilityHolder) {
            this.ability = ability;
            buttonImage.sprite = ability.sprite;
            darkMask.sprite = ability.sprite;
            coolDownDuration = ability.baseCooldown;
            _buttonPosition = ability.buttonPosition;
            UIAbilityAction(ability.buttonPosition);
            ability.Initiliaze(_abilityHolder, Player.instance.bodyTransform);
            AbilityReady();
        }

     
        private void Update() {
            notMana = NotManaCondition();
            if (notMana) return;
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (coolDownComplete) {
                AbilityReady();
            } else {
                CoolDown();
            }
        }

        private void AbilityReady() {
            coolDowntText.enabled = false;
            darkMask.enabled = false;
        }
        
        private bool notMana;
        private bool NotManaCondition() {
            if (Player.instance.mana.GetCurrentMana() < ability.manaCost) {
                noManaMask.gameObject.SetActive(true);
                return true;
            } else {
                noManaMask.gameObject.SetActive(false);
                return false;
            }
        }

        private void CoolDown() {
            coolDownTimeLeft -= Time.deltaTime;
            float roundedCd = Mathf.Round(coolDownTimeLeft);
            coolDowntText.text = roundedCd.ToString();
            darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
        }
        
        private void ButtonTriggered() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (!coolDownComplete) return;
            if (!Player.instance.mana.SpendMana(ability.manaCost)) return;
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;
            darkMask.enabled = true;
            coolDowntText.enabled = true;

            abilitiSource.clip = ability.sound;
            abilitiSource.Play();
            ability.TriggerAbility();
        }


        private AbilityButton _buttonPosition;
        public void UIAbilityAction(AbilityButton _stage, bool removeListener = false) {
            switch (_stage) {
                case AbilityButton.first:
                    if (removeListener) InputSystem.OnFirstAbility -= ButtonTriggered;
                    else InputSystem.OnFirstAbility += ButtonTriggered;
                    break;
                case AbilityButton.second:
                    if (removeListener) InputSystem.OnSecondAbility -= ButtonTriggered;
                    else InputSystem.OnSecondAbility += ButtonTriggered;
                    break;
                case AbilityButton.third:
                    if (removeListener) InputSystem.OnThirdAbility -= ButtonTriggered;
                    else InputSystem.OnThirdAbility += ButtonTriggered;
                    break;
                case AbilityButton.four:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_stage), _stage, null);
            }
        }
        
        private void OnDestroy() {
            UIAbilityAction(_buttonPosition, true);
        }
    }
}