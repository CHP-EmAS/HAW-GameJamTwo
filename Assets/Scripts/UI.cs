using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MetronomeController;

namespace Game
{
    public class UI : MonoBehaviour, ITickable
    {
        
        public void Start()
        {
            ServiceProvider.Instance.Metronome.AddTickable(this);
        }

        public void OnMetronomeTick()
        {
            
        }

        private void OnDestroy()
        {
            ServiceProvider.Instance.Metronome.RemoveTickable(this);
        }
    }
}