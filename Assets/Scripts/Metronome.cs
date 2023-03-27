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
                Instance.sources[(int)type].UnMute();

            }
        }
        
        public static void UnsubscribeOnMethod(InstrumentType type, OnNoteEvent del)
        {
            if (Instance.sources.Length > (int) type)
            {
                Instance.sources[(int)type].events -= del;
                if(Instance.sources[(int)type].events == null){
                    Instance.sources[(int)type].Mute();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

    }
}

