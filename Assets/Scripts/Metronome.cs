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
            Debug.Log(type);
            
            if (Instance.tracks.Length > (int) type)
            {
                Instance.tracks[(int)type].onHit += del;
            }
        }
        
        public static void UnsubscribeOnMethod(InstrumentType type, Utility.ArgumentelessDelegate del)
        {
            if (Instance.tracks.Length > (int) type)
            {
                Instance.tracks[(int)type].onHit -= del;
            }
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

