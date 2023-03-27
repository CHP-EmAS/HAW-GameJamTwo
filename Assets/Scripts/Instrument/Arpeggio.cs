using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Arpeggio : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
           Metronome.SubscribeOnMethod(InstrumentType.Arpeggio, OnArpeggio);
           Player.PlayerMovement.AddSpeed(scale);
           EnemyBrain.AddSpeed(scale);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Arpeggio, OnArpeggio);
           Player.PlayerMovement.DecreaseSpeed(scale);
           EnemyBrain.DecreaseSpeed(scale);
        }

        public void OnArpeggio(float diff)
        {
            
        }
    }
}