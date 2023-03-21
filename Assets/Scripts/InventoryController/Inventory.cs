using System.Collections.Generic;
using Instrument;
using MetronomeController;

namespace InventoryController
{
    public class Inventory : ITickable
    {
        private List<InstrumentBase> _availableInstruments = new List<InstrumentBase>();
        private InstrumentBase _selectedInstrument;

        public Inventory()
        {
            ServiceProvider.Instance.Metronome.AddTickable(this);
        }

        public void AddInstrument(InstrumentBase instrument)
        {
            _availableInstruments.Add(instrument);
        }

        public void RemoveInstrument(InstrumentBase instrument)
        {
            _availableInstruments.Remove(instrument);
        }

        public void SelectInstrument(InstrumentBase instrument)
        {
            _selectedInstrument = instrument;
        }

        public void OnMetronomeTick()
        {
            _selectedInstrument.PlayEffect();
        }
    }
}