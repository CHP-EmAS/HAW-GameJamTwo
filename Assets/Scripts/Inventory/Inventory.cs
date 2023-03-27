using System.Collections.Generic;
using Music.Instrument;
using UnityEngine;

namespace Music.Inventory
{
    public class Inventory : Plum.Base.Singleton<Metronome>
    {
        [SerializeField] private List<InstrumentBase> m_instruments;
        
        
    }
}