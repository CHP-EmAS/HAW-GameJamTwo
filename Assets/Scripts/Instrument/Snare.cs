using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Snare : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Snare, OnSnare);
            Weapon.Instance.SubAdditional(InstrumentType.Snare);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Snare, OnSnare);
            Weapon.Instance.UnSubAdditional(InstrumentType.Snare);
        }
        
        public void OnSnare(float diff)
        {
            
        }
    }
}