using System.Collections.Generic;

namespace MetronomeController
{
    public class Metronome
    {
        private List<ITickable> tickables = new List<ITickable>();
        
        public void Tick()
        {
            foreach (ITickable tickable in tickables)
            {
                tickable.OnMetronomeTick();
            }
        }
        
        public void AddTickable(ITickable tickable)
        {
            tickables.Add(tickable);
        }

        public void RemoveTickable(ITickable tickable)
        {
            tickables.Remove(tickable);
        }
        
    }
}

