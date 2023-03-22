using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Midi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

public class ScaleOnMidi : MonoBehaviour
{
    [SerializeField] private string midiDirectory;
    [SerializeField] private AudioSource source;
    private Vector3 initialScale, targetScale;
    private void Start(){
        initialScale = transform.localScale;
        MidiPlaySettings settings = new MidiPlaySettings();
        settings.directory = midiDirectory;
        settings.eventHandler = OnNote;
        settings.parrallelSource = source;
        MidiPlayer.PlayLooping(settings);
    }
    private void Update(){
        targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
        transform.localScale = targetScale;
    }
    private void OnNote(object sender, NotesEventArgs args){
        targetScale += initialScale * .2f;
    }
}
