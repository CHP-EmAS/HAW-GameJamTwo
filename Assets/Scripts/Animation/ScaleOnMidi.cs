using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class ScaleOnMidi : MonoBehaviour
    {
        [SerializeField] private int track = 0;
        private Vector3 initialScale, targetScale;
        private void Start()
        {
            initialScale = transform.localScale;
            Metronome.SubscribeOnMethod((Instrument.InstrumentType)track, OnNote);
        }
        private void Update()
        {
            targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
            transform.localScale = targetScale;
        }
        private void OnNote(float input)
        {
            targetScale = initialScale * 1.1f;
        }
    }

}
