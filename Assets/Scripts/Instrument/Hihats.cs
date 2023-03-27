using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Hihats : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Metronome.SubscribeOnMethod(InstrumentType.Hihats, OnHihats);
            Weapon.Instance.SubAdditional(InstrumentType.Hihats);
        }

        public override void OnRelease()
        {
            Metronome.UnsubscribeOnMethod(InstrumentType.Hihats, OnHihats);
            Weapon.Instance.UnSubAdditional(InstrumentType.Hihats);
        }
        
        public void OnHihats(float diff)
        {
            
        }
    }
}