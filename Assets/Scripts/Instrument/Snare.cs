using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Snare : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Snare, OnSnare);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Snare, OnSnare);
        }
        
        public void OnSnare(float diff)
        {
            
        }
    }
}