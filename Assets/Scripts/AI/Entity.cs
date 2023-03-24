using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Damage;

namespace Music
{
    public class Entity : PlumDamageable
    {
        private const string damageT = "_damage_t";
        [SerializeField] private ParticleSystem deathSystem;
        [SerializeField] private bool useHoldFrame = true, useDamageFWD = false, isPlayer = false;
        [SerializeField] private AudioSource hurtSource, deathSource;
        private Material mat;
        private IMoveable moveable;
        private Utility.ArgumentelessDelegate deathDelayed;
        protected override void Start()
        {        
            base.Start();
            moveable = GetComponent<IMoveable>();
            if (moveable == null) moveable = GetComponentInChildren<IMoveable>();
            mat = GetComponentInChildren<MeshRenderer>().material;
            Metronome.SubscribeOnMethod(0, ExDeath);
        }
        public override void Damage(int recievedDamage, IDamageDealer damageDealer)
        {
            base.Damage(recievedDamage, damageDealer);
            if(mat != null)
            {
                mat.SetFloat(damageT, (float)(health + .001f) / (float)(maxHealth + .001f));
            }
            if(useHoldFrame) Plum.Base.TimeManager.HoldFrame(.01f, .01f);
            if(hurtSource != null) {
                //deathDelayed += delegate{hurtSource.Play();};
            }
            if(deathSystem != null) deathSystem.Emit(5);

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
            if (mat != null) mat.SetFloat(damageT, 2.0f);
            deathDelayed = SubscribedDeath;
        }

        private void ExDeath(float i)
        {
            if(deathDelayed != null)
            {
                deathDelayed();
                deathDelayed = null;
            }
        }
        private void SubscribedDeath()
        {
            gameObject.SetActive(false);
            if (deathSystem != null)
            {
                deathSystem.transform.parent = null;
                deathSystem.Emit(30);
            }
            if (isPlayer)
            {
                Plum.Base.TimeManager.PauseGame();
                Music.UI.DeathScreen.Instance.gameObject.SetActive(true);
            }
            else
            {
                GameLoop.enemyAmount--;
            }
            Music.Player.MainCam.RequestShake(1.5f, .3f);
            if(deathSource != null){
                deathSource.Play();
            }
        }
    }
}
