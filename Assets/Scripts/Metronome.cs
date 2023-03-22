using System;
using System.Collections;
using System.Collections.Generic;
using Music.Instrument;
using UnityEngine;
using Plum.Midi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Music
{
    public class Metronome : Plum.Base.Singleton<Metronome>
    {
        [System.Serializable]
        public class MidiTrack
        {
            public string directory;
            public Utility.ArgumentelessDelegate onHit;
            public Playback playback;
        }
        public MidiTrack[] tracks;
        public static void SubscribeOnMethod(InstrumentType type, EventHandler<NotesEventArgs> del)
        {   
            if (Instance.tracks.Length > (int) type)
            {
                Instance.tracks[(int)type].playback.NotesPlaybackStarted += (o, e) => del(o, e);
            }
        }
        
        public static void UnsubscribeOnMethod(InstrumentType type, EventHandler<NotesEventArgs> del)
        {
            if (Instance.tracks.Length > (int) type)
            {
                Instance.tracks[(int)type].playback.NotesPlaybackStarted -= (o, e) => del(o, e);
            }
        }

        [SerializeField] private AudioClip clip;
        protected override void Awake()
        {
            base.Awake();
            StartMidi();
        }


        private void StartMidi(){
            AudioSource source = GetComponent<AudioSource>();
            foreach(MidiTrack track in tracks)
            {
                MidiPlaySettings settings = new MidiPlaySettings();
                settings.directory = track.directory;
                if(source != null){
                    settings.parrallelMusic = clip;
                    settings.parrallelSource = source;
                }

                MidiPlayer.PlayLooping(settings, ref track.playback);
            }
        }

    }
}

