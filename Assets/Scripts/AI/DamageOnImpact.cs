using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Damage;

namespace Music
{
    public class DamageOnImpact : MonoBehaviour, IDamageDealer
    {
        private const string playerTag = "Player";
        public GameObject GetAttached()
        {
            return gameObject;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {
                collision.gameObject.GetComponent<Entity>().Damage(1, this);
            }
        }
    }

}
