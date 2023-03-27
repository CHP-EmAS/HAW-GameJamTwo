using System;
using Music.Combat;

namespace Music.Instrument
{
    public class ThinkBreak : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
           Metronome.SubscribeOnMethod(InstrumentType.ThinkBreak, OnThinkBreak);
           Player.PlayerMovement.AddSpeed(scale);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.ThinkBreak, OnThinkBreak);
           Player.PlayerMovement.DecreaseSpeed(scale);
        }
        
        public void OnThinkBreak(float diff)
        {
            
        }
    }
}