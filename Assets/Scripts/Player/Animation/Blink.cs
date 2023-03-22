using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Multimedia;
using Plum.Midi;

namespace Music
{
    public class Blink : MonoBehaviour
    {
        [SerializeField] private string midiDirectory;
        [SerializeField] private Transform eyes;
        private Vector3 initialScale, targetScale;
        private int onNNotes = 4;
        private bool isClosed;
        private void Start()
        {
            initialScale = transform.localScale;
            return;
            MidiPlaySettings settings = new MidiPlaySettings();
            Metronome.SubscribeOnMethod(0, OnNote);
        }
        private void Update()
        {
            targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
            eyes.localScale = targetScale;
        }
        public void OnNote()
        {
            OnNote(null, null);
        }
        public void OnNote(object sender, Melanchall.DryWetMidi.Multimedia.NotesEventArgs args)
        {
            CloseEyes();
        }

        private void CloseEyes()
        {
            eyes.localScale = new Vector3(initialScale.x, 0, initialScale.z);
            targetScale = Vector3.zero;
        }
    }
}

