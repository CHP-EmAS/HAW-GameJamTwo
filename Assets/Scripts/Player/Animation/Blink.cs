using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Multimedia;
using Plum.Midi;


public class Blink : MonoBehaviour
{
    private Vector3 initialScale, targetScale;
    private void Start(){
        initialScale = transform.localScale;
        Music.Metronome.SubscribeOnMethod(0, OnNote);
    }
    private void Update(){
        targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
        transform.localScale = targetScale;
    }
    private void OnNote(object sender, NotesEventArgs args){
        targetScale = new Vector3(initialScale.x, initialScale.y, 0.0f);
    }
}

