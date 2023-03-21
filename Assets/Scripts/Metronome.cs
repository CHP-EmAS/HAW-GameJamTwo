using System.Collections;
using System.Collections.Generic;
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
        public static void SubscribeOnMethod(int index, Utility.ArgumentelessDelegate del)
        {
            Instance.tracks[index].onHit += del;
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

