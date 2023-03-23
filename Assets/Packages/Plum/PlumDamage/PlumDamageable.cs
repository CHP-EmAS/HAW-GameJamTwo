using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Damage
{
    public class PlumDamageable : MonoBehaviour, IDamageable
    {
        private const float damageCooldown = .05f;
        private float currentDamageCooldown = 0;
        [SerializeField] private bool clampHealthOnDamage = false, clampHealthOnHeal = true, stopDamageOnDeath = true;
        [SerializeField] private int maxHealth = 100;
        protected int health;
        public Utility.IntDelegate onDamageRecieved, onHealRecieved;
        public Utility.ArgumentelessDelegate onDeath, onReset;
        protected bool isDead = false;
        protected virtual void Start()
        {
            health = maxHealth;
        }

        public virtual void Reset(){
            health = maxHealth;
            isDead = false;
            onReset?.Invoke();
        }

        protected virtual void Update()
        {
            if(currentDamageCooldown > 0)
            {
                currentDamageCooldown -= Time.deltaTime;
                OnDamageT(currentDamageCooldown / GetDamageCooldown());
            } else if (currentDamageCooldown < 0)
            {
                currentDamageCooldown = 0;
                OnDamageT(0);
            }
        }

        protected virtual void OnDamageT(float percentage)
        {

        }

        protected virtual float GetDamageCooldown()
        {
            return damageCooldown;
        }

        private void ClampHealth()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
        }

        public virtual void Damage(int recievedDamage, IDamageDealer damageDealer)
        {
            //v check for cooldownx
            if (currentDamageCooldown > 0) return;


            if(stopDamageOnDeath) if (health <= 0) return;
            health -= recievedDamage;
            if(clampHealthOnDamage) ClampHealth();
            if (health <= 0) Death(damageDealer);
            else
            {
                onDamageRecieved?.Invoke(recievedDamage);
            }
            currentDamageCooldown = GetDamageCooldown();
        }

        public virtual void Damage(int recievedDamage, IDamageDealer damageDealer, Utility.ArgumentelessDelegate onDeath){
            Damage(recievedDamage, damageDealer);
            if(isDead) onDeath?.Invoke();

        }

        public virtual void Heal(int healAmount, IDamageDealer from)
        {
            health += healAmount;
            if(clampHealthOnHeal) ClampHealth();
            onHealRecieved(healAmount);
        }

        public virtual void Death(IDamageDealer from)
        {
            isDead = true;
            onDeath?.Invoke();
        }

        public virtual int GetHealth()
        {
            return health;
        }
    }
}
