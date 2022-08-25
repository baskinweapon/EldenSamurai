using System;
using Architecture.Interfaces;
using Damage;
using Enemies;
using Helpers;
using UnityEngine;

namespace Abilities {
    public class BaseAbilityCooldown: MonoBehaviour {
        public Enemy enemy;
        
        [Header("Cast object need ICastAbility")]
        public GameObject castObject;
        
        [SerializeField, Header("Parent Ability")]
        protected Transform parent;
        
        [SerializeField, Header("Main Ability")] 
        protected Ability ability;

        [SerializeField, Header("abilit Game Object")]
        private GameObject abilityHolder;
        
        [SerializeField]
        protected AudioSource abilitySource;
        
        protected float coolDownDuration;
        protected float nextReadyTime;
        protected float coolDownTimeLeft;

        private BaseDamager damager;
        private ICastAbility castAbility;

        private void OnEnable() {
            enemy.enemyAI.OnAttack += EnemyAttack;
        }

        private void Start() {
            castAbility = castObject.GetComponent<ICastAbility>();
            Initiallize(ability, abilityHolder);
        }

        private void EnemyAttack() {
            if (!CooldownComplete()) return;
            enemy.animator.SetTrigger("Attack");
            Triggered();
        }
        
        protected void Update() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (coolDownComplete) {
                AbilityReady();
            } else {
                CoolDown();
            }
            
            //casting timer
            if (isCasting) {
                time += Time.deltaTime;
                if (time >= ability.castTime) {
                    castAbility?.EndCasting();
                    isCasting = false;
                    time = 0;
                }
            }
        }

        private bool isCasting;
        private float time;
        
        public void Initiallize(Ability ability, GameObject _abilityHolder) {
            this.ability = ability;
            coolDownDuration = ability.baseCooldown;
            damager = ability.Initiliaze(_abilityHolder, parent);
            AbilityReady();
        }
        
        protected virtual void AbilityReady() {
            
        }
        
        private void CoolDown() {
            coolDownTimeLeft -= Time.deltaTime;
        }

        public float GetCooldownTimeLeft() {
            return coolDownTimeLeft;
        }

        public bool CooldownComplete() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            return coolDownComplete;
        }
        
        public void Triggered() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (!coolDownComplete) return;
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;

            castAbility?.StartCasting();
            isCasting = true;

            abilitySource.clip = ability.sound;
            abilitySource.Play();
            ability.TriggerAbility(damager);
        }

        private void OnDisable() {
            enemy.enemyAI.OnAttack -= EnemyAttack;
        }
    }
    
}