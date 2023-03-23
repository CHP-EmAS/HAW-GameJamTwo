using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Audio
{
    public delegate void OnNoteEvent(float diff);
    [RequireComponent(typeof(AudioSource))]
    public class AudiosourceAnalyzed : MonoBehaviour
    {
        private enum LogType
        {
            NONE = 0,
            ONEVENT = 1,
            ALWAYS = 2
        }

        private enum Mode
        {
            DIFFERENCE = 0,
            PERCENTAGE = 1,
            RAW = 2,
            VELOCITYDIFF = 3,
            VELOCITYPERC = 4,
        }

        [SerializeField, Range(0, 62)] private int refIndex = 62;
        [SerializeField] private float threshhold = 1f;
        [SerializeField] private LogType logType = LogType.NONE;
        [SerializeField] private Mode mode;
        [SerializeField, Range(0, .1f)] private float eventTimer = 0.0f; 
        public OnNoteEvent events;
        private const int spectrumAmount = 64;
        private AudioSource source;
        private float[] data;
        private float lastValue, lastDiff;
        private bool didEventThisNote = false;
        private void Start()
        {
            source = GetComponent<AudioSource>();
            data = new float[spectrumAmount];
        }

        public void Play(AudioClip clip, bool loop)
        {
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }

        private float refTimer = 0;
        private void Update()
        {
            source.GetSpectrumData(data, 0, FFTWindow.Rectangular);
            float v = data[refIndex];
            if (lastValue == 0) lastValue = .1f;
            float compare = 0;
            switch (mode)
            {
                case Mode.DIFFERENCE:
                    compare = lastValue - v;
                    compare *= 100;
                    break;

                case Mode.PERCENTAGE:
                    compare = (1 - (v / lastValue)) * -1;
                    break;

                case Mode.RAW:
                    compare = v;

                    break;
                case Mode.VELOCITYDIFF:
                    compare = lastValue - v;
                    lastDiff = compare;

                    compare = compare - lastValue;
                    compare *= 100;
                    break;

                case Mode.VELOCITYPERC:
                    compare = lastValue - v;
                    lastDiff = compare;

                    compare = compare / lastValue;
                    compare *= 100;
                    break;
            }
            lastValue = v;
            if (logType == LogType.ALWAYS)
            {
                Debug.Log(compare);
            }

            if(refTimer > 0)
            {
                refTimer -= Time.deltaTime;
                return;
            }
            if(compare > threshhold)
            {
                if(!didEventThisNote) Event(compare);
                didEventThisNote = true;
                if (eventTimer > 0) refTimer = eventTimer;
            }
            else
            {
                didEventThisNote = false;
            }
        }


        private void Event(float input)
        {
            if(logType == LogType.ONEVENT)
            {
                Debug.Log(input);
            }
            events?.Invoke(input);
        }
    }
}

