using System;
using Music.Instrument;
using UnityEngine;

namespace Music.Combat
{
    public struct ProjectileInitSettings
    {
        
    }
    
    public class Projectile : MonoBehaviour
    {
        public static ProjectileDelegate OnCollision;
        public static TrackDelegate OnTrack;
        
        public delegate void ProjectileDelegate(Projectile instance);
        public delegate void TrackDelegate(Projectile instance, InstrumentType type);

        private float _speed;
        private Vector3 _direction;

        private void Initialize()
        {
            gameObject.SetActive(true);
            
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnKick);
            Metronome.SubscribeOnMethod(InstrumentType.Piano, OnPiano);
            Metronome.SubscribeOnMethod(InstrumentType.Flute, OnFlute);
        }
        
        private void Release()
        {
            gameObject.SetActive(false);
            
            Metronome.UnsubscribeOnMethod(InstrumentType.Kick, OnKick);
            Metronome.UnsubscribeOnMethod(InstrumentType.Piano, OnPiano);
            Metronome.UnsubscribeOnMethod(InstrumentType.Flute, OnFlute);
        }
        
        public void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(this);
        }

        public void Explode()
        {
            
        }
        
        public void Wave()
        {
            
        }

        private void OnKick()
        {
            OnTrack(this, InstrumentType.Kick);
        }
        
        private void OnPiano()
        {
            OnTrack(this, InstrumentType.Piano);
        }
        
        private void OnFlute()
        {
            OnTrack(this, InstrumentType.Flute);
        }
    }
}