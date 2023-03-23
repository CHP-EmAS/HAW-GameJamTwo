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
        [SerializeField] private LayerMask damageLayers;
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
            
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnKick);
            //Metronome.SubscribeOnMethod(InstrumentType.Piano, OnPiano);
            //Metronome.SubscribeOnMethod(InstrumentType.Flute, OnFlute);
            
            gameObject.SetActive(true);
        }
        
        public void Release()
        {
            gameObject.SetActive(false);
            
            Metronome.UnsubscribeOnMethod(InstrumentType.Kick, OnKick);
            //Metronome.UnsubscribeOnMethod(InstrumentType.Piano, OnPiano);
            //dwwdsMetronome.UnsubscribeOnMethod(InstrumentType.Flute, OnFlute);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void UpdateVelocity()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }

        protected virtual void OnDealt()
        {
            Release();
        }

        private void TryDamage(GameObject target)
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
            }
        }

        private void CheckDamageable()
        {
            if(Physics.Raycast(transform.position, lastPositionWS - transform.position, out RaycastHit hit, damageLayers))
            {
                if(hit.collider != null)
                {
                    TryDamage(hit.collider.gameObject);
                }
            }
        }

        private void FixedUpdate()
        {
            UpdateVelocity();
            CheckDamageable();
            lastPositionWS = transform.position;
        }

        public void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(this);
            TryDamage(other.gameObject);
        }

        public void Explode()
        {
            _releaseAction(this);
        }
        
        public void Wave()
        {
            
        }

        private void OnKick(float input)
        {
            OnTrack?.Invoke(this, InstrumentType.Kick);
        }
        
        private void OnPiano(float input)
        {
            OnTrack?.Invoke(this, InstrumentType.Piano);
        }
        
        private void OnFlute(float input)
        {
            OnTrack?.Invoke(this, InstrumentType.Flute);
        }

        private void OnDestroy(float input)
        {
            Release();
        }
    }
}