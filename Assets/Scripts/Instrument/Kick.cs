using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Kick : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnKick);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Kick, OnKick);
        }
        
        public void OnKick(float diff)
        {
            
        }
    }
}