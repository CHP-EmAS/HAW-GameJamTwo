using System;
using Music.Instrument;
using UnityEngine;

namespace Music.Combat
{
    public class Projectile : MonoBehaviour
    {
        public static ProjectileDelegate OnCollision;
        public static TrackDelegate OnTrack;

        public delegate void ProjectileDelegate(Projectile instance);
        public delegate void TrackDelegate(Projectile instance, InstrumentType type);
        
        private void Awake()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnKick);
            Metronome.SubscribeOnMethod(InstrumentType.Piano, OnPiano);
            Metronome.SubscribeOnMethod(InstrumentType.Flute, OnFlute);
            
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