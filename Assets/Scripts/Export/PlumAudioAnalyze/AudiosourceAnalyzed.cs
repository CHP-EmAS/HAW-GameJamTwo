using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
            ONBACK = 2,
            ALWAYS = 3
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
        [HideInInspector] public float backThreshhold = .05f;
        [SerializeField] private LogType logType = LogType.NONE;
        [SerializeField] private Mode mode;
        [SerializeField, Range(0, .1f)] private float eventTimer = 0.0f; 
        [SerializeField] private bool startMute = false;
        [SerializeField] private bool useAbsouluteValue = false;
        [SerializeField] private bool useSpectrumData;
        [SerializeField] private float threshhold = 1f; public float Threshhold => threshhold;
        public OnNoteEvent events;
        private const int spectrumAmount = 64;
        private AudioSource source;
        private float[] data; public float[] GetData() => data;
        private float lastValue, lastDiff;
        private bool didEventThisNote = false;
        private float initialVolume;
        private void Start()
        {
            source = GetComponent<AudioSource>();
            data = new float[spectrumAmount];
            initialVolume = source.volume;
            if(startMute) Mute();
        }

        public void Mute(){
            source.volume = 0.0f;
        }

        public void UnMute(){
            source.volume = initialVolume;
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
            if(useSpectrumData) source.GetSpectrumData(data, 0, FFTWindow.Rectangular);
            else source.GetOutputData(data, 0);
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
            if(useAbsouluteValue) compare = Mathf.Abs(compare);
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
            else if(compare <= backThreshhold)
            {
                if(logType == LogType.ONBACK) Debug.Log(compare);
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

#if UNITY_EDITOR
    [CustomEditor(typeof(AudiosourceAnalyzed))]
    [CanEditMultipleObjects]
    public class SourceInspector : Plum.Base.CustomInspector<AudiosourceAnalyzed>{
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Lower Threshhold");
            Item.backThreshhold = EditorGUILayout.Slider(Item.backThreshhold, -Mathf.Abs(Item.Threshhold), Mathf.Abs(Item.Threshhold));
            EditorGUILayout.LabelField("Data Visualization");
            AnimationCurve curve = new AnimationCurve();
            float[] data = Item.GetData();
            if(data == null) return;
            for (int i = 0; i < data.Length; i++)
            {
                curve.AddKey(i, data[i]);
            }
            EditorGUILayout.CurveField(curve);
        }
    }
#endif
}

