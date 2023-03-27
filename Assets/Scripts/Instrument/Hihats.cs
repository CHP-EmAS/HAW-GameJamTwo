using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Hihats : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Hihats, OnHihats);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Hihats, OnHihats);
        }
        
        public void OnHihats(float diff)
        {
            
        }
    }
}