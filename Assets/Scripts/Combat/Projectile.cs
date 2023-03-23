using System;
using Music.Instrument;
using UnityEngine;

namespace Music.Combat
{
    public struct ProjectileInitSettings
    {
        public Action<Projectile> ReleaseAction;
        public Transform SpawnTransform;
        public float Speed;
        public float LifeSpan;
    }
  
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public static ProjectileDelegate OnCollision;
        public static TrackDelegate OnTrack;
        
        public delegate void ProjectileDelegate(Projectile instance);
        public delegate void TrackDelegate(Projectile instance, InstrumentType type);

        private Rigidbody _rigidbody;
        
        private Action<Projectile> _releaseAction;
        private float _speed;
        private float _lifeSpan;
        private Vector3 _direction;

        public void Initialize(ProjectileInitSettings settings)
        {
            _releaseAction = settings.ReleaseAction;
            
            transform.position = settings.SpawnTransform.position;
            transform.rotation = settings.SpawnTransform.rotation;

            _lifeSpan = settings.LifeSpan;
            _speed = settings.Speed;
            
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

        private void Start()
        {
            _rigidbody.AddForce(_rigidbody.transform.forward * _speed);
        }

        public void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(this);
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