using System.Globalization;
using Architecture.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities {
    public class PlayerAbilityCooldown : MonoBehaviour {
        [Header("Cast object need ICastAbility")]
        public GameObject castObject;
        
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
        
        private ICastAbility castAbility;
        private void Start() {
            if (!ability) this.enabled = false;
            Initiallize(ability, abilityHolder);
            castAbility = castObject.GetComponent<ICastAbility>();
        }
        
        private void Initiallize(Ability _ability, GameObject _abilityHolder) {
            ability = _ability;
            buttonImage.sprite = ability.sprite;
            darkMask.sprite = ability.sprite;
            coolDownDuration = ability.baseCooldown;
            _buttonPosition = ability.buttonPosition;
            InputSlotAction(ability.buttonPosition);
            ability.Initiliaze(_abilityHolder, Player.instance.abilityContainer);
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
        
        private bool isCasting;
        private float time;
        private void LateUpdate() {
            if (!isCasting) return;
            time += Time.deltaTime;
            if (time >= ability.castTime) {
                isCasting = false;
                castAbility.EndCasting();
                time = 0;
            }
            Player.instance.SetCastingState(isCasting);
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
            coolDowntText.text = roundedCd.ToString(CultureInfo.InvariantCulture);
            darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
        }
        
        private void ButtonTriggered() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (!coolDownComplete) return;
            if (Player.instance.IsCastingAbility()) return;
            if (!Player.instance.mana.SpendMana(ability.manaCost)) return;
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;
            darkMask.enabled = true;
            coolDowntText.enabled = true;

            isCasting = true;
            castAbility.StartCasting();

            abilitiSource.clip = ability.sound;
            abilitiSource.Play();
            ability.TriggerAbility();
        }


        private AbilityButton _buttonPosition;
        private void InputSlotAction(AbilityButton _stage, bool removeListener = false) {
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
                    if (removeListener) InputSystem.OnFourthAbility -= ButtonTriggered;
                    else InputSystem.OnFourthAbility += ButtonTriggered;
                    break;
            }
        }
        
        private void OnDestroy() {
            InputSlotAction(_buttonPosition, true);
        }
    }
}