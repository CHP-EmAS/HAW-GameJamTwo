using System.Collections;
using System.Collections.Generic;
using Music.Instrument;
using UnityEngine;
using Plum.Midi;

namespace Music
{
    public class Metronome : Plum.Base.Singleton<Metronome>
    {
        [System.Serializable]
        public struct MidiTrack
        {
            public string directory;
            public Utility.ArgumentelessDelegate onHit;
        }
        public MidiTrack[] tracks;
        public static void SubscribeOnMethod(InstrumentType type, Utility.ArgumentelessDelegate del)
        {
            Instance.tracks[(int)type].onHit += del;
        }

        private void Start()
        {
            foreach(MidiTrack track in tracks)
            {
                void LocalPlay(object sender, Melanchall.DryWetMidi.Multimedia.NotesEventArgs args)
                {
                    track.onHit?.Invoke();
                }

                MidiPlaySettings settings = new MidiPlaySettings();
                settings.directory = track.directory;
                settings.eventHandler += LocalPlay;

                MidiPlayer.PlayLooping(settings);
            }
        }

    }
}

