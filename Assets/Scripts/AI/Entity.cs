using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Damage;

namespace Music
{
    public class Entity : PlumDamageable
    {
        private IMoveable moveable;
        protected override void Start()
        {
            base.Start();
            moveable = GetComponent<IMoveable>();
            if (moveable == null) moveable = GetComponentInChildren<IMoveable>();
        }
        public override void Damage(int recievedDamage, IDamageDealer damageDealer)
        {
            Vector3 tarPos = damageDealer.GetAttached().transform.position;
            tarPos.y = 0.0f;
            Vector3 dir = new Vector3(transform.position.x, 0.0f, transform.position.y) - tarPos;
            moveable.AddForce(dir.normalized * 5);
            base.Damage(recievedDamage, damageDealer);
        }
        public override void Death(IDamageDealer source)
        {
            gameObject.SetActive(false);
        }
    }
}
