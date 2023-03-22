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
    private Utility.ArgumentelessDelegate del;
    private Vector3 initialScale, targetScale;
    private void Start(){
        //v for whatever reason this breaks everything
        del += FindObjectOfType<Music.Blink>().OnNote;

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
    private void OnNote()
    {
        OnNote(null, null);
    }
    private void OnNote(object sender, NotesEventArgs args){
        del?.Invoke();
        targetScale += initialScale * .2f;
    }
}
