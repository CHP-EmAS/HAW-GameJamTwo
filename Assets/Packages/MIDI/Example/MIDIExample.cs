using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Midi{
    public class MIDIExample : MonoBehaviour
    {
        [SerializeField] private string midiDirectory = "Assets/MIDI/Example/Example.mid";
        [SerializeField] private bool playLoop = false;
        private void Start(){
            if(playLoop){
                MidiPlayer.PlayLooping(midiDirectory, OnNotePlayed);
            } else{
                MidiPlayer.PlaySingle(midiDirectory, OnNotePlayed);
            }
        }
        private void OnNotePlayed(object sender, Melanchall.DryWetMidi.Multimedia.NotesEventArgs args){
            Debug.Log("I played a note!:)");
        }
    }
}
