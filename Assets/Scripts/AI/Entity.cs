using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Damage;

namespace Music
{
    public class Entity : PlumDamageable
    {
        [SerializeField] private bool useHoldFrame = true, useDamageFWD = false;
        private IMoveable moveable;
        protected override void Start()
        {
            base.Start();
            moveable = GetComponent<IMoveable>();
            if (moveable == null) moveable = GetComponentInChildren<IMoveable>();
        }
        public override void Damage(int recievedDamage, IDamageDealer damageDealer)
        {
            base.Damage(recievedDamage, damageDealer);
            if(useHoldFrame) Plum.Base.TimeManager.HoldFrame(.01f, .01f);

            if (useDamageFWD)
            {
                moveable.AddForce(-damageDealer.GetAttached().transform.forward * (25.0f / (recievedDamage + .01f)));
                return;
            }
            Vector3 tarPos = damageDealer.GetAttached().transform.position;
            tarPos.y = 0.0f;
            Vector3 dir = new Vector3(transform.position.x, 0.0f, transform.position.z) - tarPos;
            moveable.AddForce(dir.normalized * 10.5f);
        }
        public override void Death(IDamageDealer source)
        {
            gameObject.SetActive(false);
        }
    }
}
