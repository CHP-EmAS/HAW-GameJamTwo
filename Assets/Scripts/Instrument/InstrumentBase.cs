using System;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Instrument
{
    public enum InstrumentType : int
    {
        Invalid = -1,
        Kick = 0,
        Snare = 1,
        Hihats = 2,
        Bass = 3,
        Arpeggio = 4,
        MainSynth = 5,
        ThinkBreak = 6,
    }
    
    public abstract class InstrumentBase : MonoBehaviour
    {
        protected const float scale = .25f;
        [SerializeField] protected Sprite m_uiInstrumentSprite;
        [SerializeField] protected int m_trackNumber;

        public abstract void OnSubscribe();
        public abstract void OnRelease();
    }
}