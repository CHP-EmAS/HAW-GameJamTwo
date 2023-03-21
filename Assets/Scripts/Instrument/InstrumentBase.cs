using System;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Instrument
{
    public abstract class InstrumentBase : MonoBehaviour
    {
        [SerializeField] protected Sprite m_uiInstrumentSprite;
        [SerializeField] protected int m_trackNumber;

        public abstract void OnSubscribe();
        public abstract void OnRelease();
    }
}