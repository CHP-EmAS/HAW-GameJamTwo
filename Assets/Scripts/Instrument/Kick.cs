using System;
using Music.Combat;

namespace Music.Instrument
{
    public class Kick : InstrumentBase
    {
        
        public override void OnSubscribe()
        {
            Weapon.Instance.SubAdditional(InstrumentType.Kick);
        }

        public override void OnRelease()
        {
            Weapon.Instance.UnSubAdditional(InstrumentType.Kick);
        }
        
    }
}