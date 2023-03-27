using System;
using System.Collections;
using System.Collections.Generic;
using Music.Instrument;
using UnityEngine;
using Plum.Audio;

namespace Music
{
    public class Metronome : Plum.Base.Singleton<Metronome>
    {
        [SerializeField] private AudiosourceAnalyzed[] sources;
        public static void SubscribeOnMethod(InstrumentType type, OnNoteEvent del)
        {   
            if (Instance.sources.Length > (int) type)
            {
                Instance.sources[(int)type].events += del;
            }
        }
        
        public static void UnsubscribeOnMethod(InstrumentType type, OnNoteEvent del)
        {
            if (Instance.sources.Length > (int) type)
            {
                Instance.sources[(int)type].events -= del;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

    }
}

