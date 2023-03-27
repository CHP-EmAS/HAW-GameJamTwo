using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Arpeggio : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
           Metronome.SubscribeOnMethod(InstrumentType.Arpeggio, OnArpeggio);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Arpeggio, OnArpeggio);
        }

        public void OnArpeggio(float diff)
        {
            
        }
    }
}