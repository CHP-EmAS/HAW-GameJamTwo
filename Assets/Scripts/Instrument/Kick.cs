using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Kick : InstrumentBase
    {
        public Kick()
        {
            
        }
        
        private void Awake()
        {
            
        }

        private void OnNote()
        {
            
        }

        public override void OnSubscribe()
        {
            Projectile.OnCollision += ExecuteExplosion;
        }

        public override void OnRelease()
        {
            Projectile.OnCollision -= ExecuteExplosion;
        }

        private void ExecuteExplosion(Projectile projectile)
        {
            projectile.Explode();
        }
    }
}