using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Midi{
    public class MIDIExample : MonoBehaviour
    {
        [SerializeField] private string midiDirectory = "Assets/MIDI/Example/Example.mid";
        [SerializeField] private bool playLoop = false;
        private void Start(){
            MidiPlaySettings settings = new MidiPlaySettings();
            settings.directory = midiDirectory;
            settings.eventHandler = OnNotePlayed;
            if(playLoop){
                MidiPlayer.PlayLooping(settings);
            } else{
                MidiPlayer.PlaySingle(settings);
            }
        }
        private void OnNotePlayed(object sender, Melanchall.DryWetMidi.Multimedia.NotesEventArgs args){
            Debug.Log("I played a note!:)");
        }
    }
}
