using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Midi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

public class ScaleOnMidi : MonoBehaviour
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
        Debug.Log("Am doing");
        targetScale += initialScale * .2f;
    }
}
