using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Midi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

public class ScaleOnMidi : MonoBehaviour
{
    [SerializeField] private string midiDirectory;
    [SerializeField] private MidiPlayer reference;
    [SerializeField] private AudioSource source;
    private Vector3 initialScale, targetScale;
    private void Start(){
        initialScale = transform.localScale;
        MidiPlayer.PlayLooping(midiDirectory, OnNote, this, 1.0f);
        source.Play();
    }
    private void Update(){
        targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
        transform.localScale = targetScale;
    }
    private void OnNote(object sender, NotesEventArgs args){
        Debug.Log("Shall");
        targetScale += initialScale * .2f;
    }
}
