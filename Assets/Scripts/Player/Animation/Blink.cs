using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blink : MonoBehaviour
{
    private const int threshhold = 12;
    private int counter;
    private Vector3 initialScale, targetScale;
    private void Start(){
        initialScale = transform.localScale;
        Music.Metronome.SubscribeOnMethod(0, OnNote);
    }
    private void Update(){
        targetScale = Vector3.Lerp(targetScale, initialScale, 5 * Time.deltaTime);
        transform.localScale = targetScale;
    }
    private void OnNote(float input){
        counter++;
        if(counter % threshhold != 0) return;
        targetScale = new Vector3(initialScale.x, initialScale.y, 0.0f);
    }
}

