using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Damage
{
    public interface IDamageable
    {
        public int GetHealth();
        public void Damage(int recievedDamage, IDamageDealer damageable);
        public void Damage(int recievedDamage, IDamageDealer damageable, Utility.ArgumentelessDelegate onDeath);
        public void Heal(int recievedHeal, IDamageDealer from);
    }

    public interface IDamageDealer{
        public GameObject GetAttached();
    }
}
