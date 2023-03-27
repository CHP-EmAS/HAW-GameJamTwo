using System;
using System.Collections.Generic;
using Music.Instrument;

namespace Music.Inventory
{
    public class Inventory : Plum.Base.Singleton<Inventory>
    {
        private Dictionary<InstrumentType, InstrumentBase> m_availableInstruments = new Dictionary<InstrumentType, InstrumentBase>() {
            {InstrumentType.Snare     , new Snare()},
            {InstrumentType.Hihats    , new Hihats()},
            {InstrumentType.Bass      , new Bass()},
            {InstrumentType.Arpeggio  , new Arpeggio()},
            {InstrumentType.MainSynth , new MainSynth()},
            {InstrumentType.ThinkBreak, new ThinkBreak()}
        };

        private Random _random = new Random();
        
        private const int MaxPickedInstruments = 3;
        private List<InstrumentType> _pickedInstruments;
        
        private List<InstrumentType> _unpickedInstruments = new List<InstrumentType>() {
            InstrumentType.Snare, 
            InstrumentType.Hihats,    
            InstrumentType.Bass,     
            InstrumentType.Arpeggio,  
            InstrumentType.MainSynth, 
            InstrumentType.ThinkBreak
        };
        
        public void CollectNextInstrument()
        {
            InstrumentType randomUnusedInstrument = GetRandomUnusedInstrument();
            m_availableInstruments[randomUnusedInstrument].OnSubscribe();
            
            if (_pickedInstruments.Count >= MaxPickedInstruments) {
                UnsubscribeRandomInstrument();
            }
        
            _pickedInstruments.Add(randomUnusedInstrument);
        }
        
        public InstrumentType GetRandomUnusedInstrument()
        {
            if (_unpickedInstruments.Count > 0)
            {
                int randomIndex = _random.Next(0, _unpickedInstruments.Count);
                InstrumentType randomInstrument = _unpickedInstruments[randomIndex];
                _unpickedInstruments.RemoveAt(randomIndex);
                return randomInstrument;
            }

            return InstrumentType.Invalid;
        }
        
        public void UnsubscribeRandomInstrument()
        {
            if (_pickedInstruments.Count > 0)
            {
                int randomIndex = _random.Next(0, _pickedInstruments.Count);
                InstrumentType randomInstrument = _pickedInstruments[randomIndex];
                m_availableInstruments[randomInstrument].OnRelease();
                _pickedInstruments.RemoveAt(randomIndex);
            }
        }
    }
}