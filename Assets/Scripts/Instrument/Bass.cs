using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Bass : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Bass, OnBass);
            EnemyBrain.DecreaseSpeed(scale);
            Player.PlayerMovement.DecreaseSpeed(scale);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Bass, OnBass);
            Player.PlayerMovement.AddSpeed(scale);
           EnemyBrain.AddSpeed(scale);
        }
        
        public void OnBass(float diff)
        {
            
        }
    }
}