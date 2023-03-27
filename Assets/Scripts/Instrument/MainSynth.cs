using System;
using Music.Combat;

namespace Music.Instrument
{
    public class MainSynth : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.MainSynth, OnMainSynth);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.MainSynth, OnMainSynth);
        }

        public void OnMainSynth(float diff)
        {
            
        }
    }
}