using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Plum.Midi{
    public delegate void OnNote(object sender, NotesEventArgs args);

    //v this class represents a singleton midi player
    public class MidiPlayer : MonoBehaviour
    {
        private static List<Playback> allPlayBacks = new List<Playback>();
        private static MidiPlayer instance;
        public static MidiPlayer Instance{get => instance;}
        private void Awake(){
            if(instance != null){
                Debug.LogError("Tried to create a second instance of the singleton 'Midiplayer'");
                Destroy(this);
                return;
            }
            instance = this;
        }

#region UTILS
        private static Playback GetPlayback(MidiFile file){
            return file.GetPlayback(new PlaybackSettings{ ClockSettings = new MidiClockSettings{
            CreateTickGeneratorCallback = () => new RegularPrecisionTickGenerator()}}
            );
        }
        private static void ConfigPlayBack(Playback playback, float speed, OnNote eventHandler){
            playback.Speed = speed;
            playback.NotesPlaybackStarted += (sender, e) => eventHandler?.Invoke(sender, e);
            playback.InterruptNotesOnStop = true;
            allPlayBacks.Add(playback);
        }
#endregion //!Utils

#region PLAYBACK
        //Play a midi sequence once
        public static void PlaySingle(string midiDirectory, OnNote eventHandler, 
        MonoBehaviour coroutineParent = null, float speed = 1.0f){
            Playback p = null;
            PlaySingle(midiDirectory, eventHandler, ref p, coroutineParent, speed);
        }
        public static void PlaySingle(string midiDirectory, OnNote eventHandler, ref Playback playback, MonoBehaviour coroutineParent = null, float speed = 1.0f){
            MidiFile midiF = MidiFile.Read(midiDirectory);
            playback = GetPlayback(midiF);
            ConfigPlayBack(playback, speed, eventHandler);

            if(coroutineParent == null) coroutineParent = instance;
            coroutineParent.StartCoroutine(PlayMusic(playback));
        }

        //v use this to play looping midi files
        public static void PlayLooping(string midiDirectory, OnNote eventHandler, MonoBehaviour coroutineParent = null, float speed = 1.0f){
            Playback p = null;
            PlayLooping(midiDirectory, eventHandler, ref p, coroutineParent, speed);
        }

        public static void PlayLooping(string midiDirectory, OnNote eventHandler, ref Playback playback, MonoBehaviour coroutineParent = null, float speed = 1.0f){
            MidiFile midiF = MidiFile.Read(midiDirectory);
            playback = GetPlayback(midiF);
            ConfigPlayBack(playback, speed, eventHandler);
            playback.Loop = true;

            if(coroutineParent == null) coroutineParent = instance;
            allPlayBacks.Add(playback);
            coroutineParent.StartCoroutine(PlayMusicLooped(playback));
        }

        //v use this to stop all current loops in this instance
        public static void StopAllLoops(){
            instance.StopAllCoroutines();
        }

        //v stops playback of one specific instance
        public static void StopPlayBack(Playback p, bool dispose = false){
            p.Stop();
            if(dispose) p.Dispose();
        }

        //v use as debug
        public static void DebugNotePlayed(object sender, NotesEventArgs args){
            Debug.Log("dev - Note was played");
        }

#endregion //!PLAYBACK

#region COROUTINES
        private static IEnumerator PlayMusicLooped(Playback p){
            p.Start();
            while(p.IsRunning){
                yield return new WaitForEndOfFrame();
            }
            p.Dispose();
        }

        private static IEnumerator PlayMusic(Playback p){
            p.Start();
            while(p.IsRunning){
                yield return new WaitForEndOfFrame();
            }
            allPlayBacks.Remove(p);
            p.Dispose();
        }
#endregion //!COROUTINES

        private void OnDestroy(){
            foreach(var n in allPlayBacks){
                try{
                    n.Stop();
                    n.Dispose();
                }
                catch{
                    continue;
                }
            }
            allPlayBacks.Clear();
            allPlayBacks = new List<Playback>();
        }
    }

}
