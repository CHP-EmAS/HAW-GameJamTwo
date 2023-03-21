using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Sound{
    //v overarching UI Soundsource-system
    public class BUI_SoundMaster : Plum.Base.Singleton<BUI_SoundMaster>
    {
        [SerializeField] private int startSources = 2;
        private List<AudioSource> sources = new List<AudioSource>();

        private void Start(){
            for (int i = 0; i < startSources; i++)
            {
                AddSource();
            }
        }
        public void PlaySound(AudioClip clip){
            FreeSource().PlayFastOneShotSafe(clip);
        }

        private AudioSource AddSource(){
            AudioSource s = gameObject.AddComponent<AudioSource>();
            s.loop = false;
            s.playOnAwake = false;
            sources.Add(s);
            return s;
        }
        public AudioSource GetSource(){
            return AddSource();
        }

        //v we want to use multiple ones to allow for parallel sounds
        private AudioSource FreeSource(){
            AudioSource selected = null;
            foreach (AudioSource item in sources)
            {
                selected = item;
                if(!selected.isPlaying) break;      //<- if free break
            }
            return selected;
        }
    }
}
