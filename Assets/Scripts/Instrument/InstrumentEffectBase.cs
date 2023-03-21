using UnityEngine;

namespace Instrument
{
    public abstract class InstrumentBase : MonoBehaviour
    {
        private Sprite _instrumentSprite;
        public abstract void PlayEffect();
    }
}