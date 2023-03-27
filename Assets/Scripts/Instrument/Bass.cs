using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Bass : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Bass, OnBass);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Bass, OnBass);
        }
        
        public void OnBass(float diff)
        {
            
        }
    }
}