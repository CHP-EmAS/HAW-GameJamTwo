using System;
using Music.Instrument;
using UnityEngine;
using Plum.Damage;

namespace Music.Combat
{
    [System.Serializable]
    public class ProjectileInitSettings
    {
        public Action<Projectile> ReleaseAction;
        public Transform SpawnTransform;
        public float Speed = 10.0f;
        public float LifeSpan = 5.0f;
        public int damage = 25;
        public string damageTag = "Enemy";
    }
  
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IDamageDealer
    {
        public GameObject GetAttached() => gameObject;
        [SerializeField] private float checkDST = 2.0f;
        [SerializeField] private LayerMask damageLayers, damageNGround;
        public static ProjectileDelegate OnCollision;
        public static TrackDelegate OnTrack;
        
        public delegate void ProjectileDelegate(Projectile instance);
        public delegate void TrackDelegate(Projectile instance, InstrumentType type);

        private ProjectileInitSettings currentSettings;
        private Rigidbody _rigidbody;
        
        private Action<Projectile> _releaseAction;
        private float _speed;
        private float _lifeSpan;
        private Vector3 _direction;

        private Vector3 lastPositionWS;

        public void Initialize(ProjectileInitSettings settings)
        {
            _releaseAction = settings.ReleaseAction;
            
            transform.position = settings.SpawnTransform.position;
            transform.rotation = settings.SpawnTransform.rotation;

            _lifeSpan = settings.LifeSpan;
            _speed = settings.Speed;
            currentSettings = settings;
            
            gameObject.SetActive(true);
            lastPositionWS = transform.position;
        }
        
        public void Release()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            lastPositionWS = transform.position;
        }

        private void UpdateVelocity()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }

        protected virtual void OnDealt()
        {
            Release();
        }

        private void TryDamage(GameObject target, bool disableOnNot = false)
        {
            if (target.CompareTag(currentSettings.damageTag))
            {
                IDamageable damage;
                target.TryGetComponent<IDamageable>(out damage);
                if(damage != null)
                {
                    damage.Damage(currentSettings.damage, this);
                    OnDealt();
                }
            } else{
                if(disableOnNot) {
                    OnDealt();
                }
            }
        }

        private void CheckDamageable()
        {
            if(Physics.Raycast(transform.position, lastPositionWS - transform.position, out RaycastHit hit, (lastPositionWS - transform.position).magnitude, damageNGround))
            {
                if(hit.collider != null)
                {
                    TryDamage(hit.collider.gameObject, true);
                }
            }
        }

        private void CheckDamageableSphere(){
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, checkDST, Vector3.forward, checkDST, damageLayers);
            if(hits.Length != 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if(hit.collider != null)
                    {
                        TryDamage(hit.collider.gameObject);
                    }
                }

            }
        }


        private void FixedUpdate()
        {
            UpdateVelocity();
            CheckDamageable();
            CheckDamageableSphere();
            lastPositionWS = transform.position;
        }

        public void Explode()
        {
            _releaseAction(this);
        }
        
        public void Wave()
        {
            
        }

        private void OnDrawGizmos(){
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, .2f);
            Gizmos.DrawSphere(transform.position, checkDST);
        }
    }
}