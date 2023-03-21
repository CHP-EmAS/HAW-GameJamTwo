using System;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Instrument
{
    public enum InstrumentType : int
    {
        Kick = 0,
        Piano = 1,
        Flute = 2,
    }
    
    public abstract class InstrumentBase : MonoBehaviour
    {
        [SerializeField] protected Sprite m_uiInstrumentSprite;
        [SerializeField] protected int m_trackNumber;

        public abstract void OnSubscribe();
        public abstract void OnRelease();
    }
}