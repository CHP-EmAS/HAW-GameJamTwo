using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Kick : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Projectile.OnCollision += ExecuteExplosion;
            Projectile.OnTrack += ExecuteWave;
        }

        public override void OnRelease()
        {
            Projectile.OnCollision -= ExecuteExplosion;
        }

        private void ExecuteExplosion(Projectile projectile)
        {
            projectile.Explode();
        }
        
        private void ExecuteWave(Projectile projectile, InstrumentType type)
        {
            if (type == InstrumentType.Kick)
            {
                projectile.Wave();
            }
        }
    }
}