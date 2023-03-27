using System;
using Music.Combat;

namespace Music.Instrument
{
    public class ThinkBreak : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.ThinkBreak, OnThinkBreak);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.ThinkBreak, OnThinkBreak);
        }
        
        public void OnThinkBreak(float diff)
        {
            
        }
    }
}