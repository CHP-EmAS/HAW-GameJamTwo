using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace Plum.Midi{
    public delegate void OnNote(object sender, NotesEventArgs args);
    public class MidiPlaySettings
    {
        public string directory;
        public OnNote eventHandler;
        public MonoBehaviour coroutineParent;
        public float speed = 1.0f;
        public AudioSource parrallelSource;
        public AudioClip parrallelMusic;
    }

    //v this class represents a singleton midi player
    public class MidiPlayer : MonoBehaviour
    {
        private class SubscribedSource
        {
            public AudioSource source;
            public AudioClip clip;
            public void Play()
            {
                Debug.Log("upper clip is working" + "  " + source);
                if (clip != null) source.clip = clip;
                source.Stop();
                source.Play();
                Debug.Log("Should replay" + " " + clip);
            }
            public SubscribedSource(AudioSource source, Playback playback, AudioClip clip = null)
            {
                this.source = source;
                this.clip = clip;
            }
        }

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
        private static void ConfigPlayBack(Playback playback, MidiPlaySettings settings){
            if(settings.speed >= 0) playback.Speed = settings.speed;
            playback.NotesPlaybackStarted += (sender, e) => settings.eventHandler?.Invoke(sender, e);
            playback.InterruptNotesOnStop = true;
            allPlayBacks.Add(playback);
        }
#endregion //!Utils

#region PLAYBACK
        //Play a midi sequence once
        public static void PlaySingle(MidiPlaySettings settings){
            Playback p = null;
            PlaySingle(settings, ref p);
        }
        public static void PlaySingle(MidiPlaySettings settings, ref Playback playback){
            MidiFile midiF = MidiFile.Read(settings.directory);
            playback = GetPlayback(midiF);
            ConfigPlayBack(playback, settings);

            if(settings.coroutineParent == null) settings.coroutineParent = instance;
            settings.coroutineParent.StartCoroutine(PlayMusic(playback));
        }

        //v use this to play looping midi files
        public static void PlayLooping(MidiPlaySettings settings){
            Playback p = null;
            PlayLooping(settings, ref p);
        }

        public static void PlayLooping(MidiPlaySettings settings, ref Playback playback){
            MidiFile midiF = MidiFile.Read(settings.directory);
            playback = GetPlayback(midiF);
            //playback.Loop = true;
            ConfigPlayBack(playback, settings);

            if(settings.coroutineParent == null) settings.coroutineParent = instance;
            allPlayBacks.Add(playback);
            SubscribedSource sSource = new SubscribedSource(settings.parrallelSource, playback, settings.parrallelMusic);
            settings.coroutineParent.StartCoroutine(PlayMusicLooped(playback, sSource));
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
        private static IEnumerator PlayMusicLooped(Playback p, SubscribedSource source = null){
            p.Start();
            while(p.IsRunning){
                yield return new WaitForEndOfFrame();
            }
            yield return PlayMusicLooped(p, source);
        }

        private static IEnumerator PlayMusic(Playback p){
            p.Start();
            while (p.IsRunning){
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
