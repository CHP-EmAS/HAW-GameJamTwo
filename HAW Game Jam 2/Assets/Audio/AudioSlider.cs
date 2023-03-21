using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    //public void Start()
    //{
    //    mixer.GetFloat("MusicVolume")
    //}

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue)*20);
    }
}
