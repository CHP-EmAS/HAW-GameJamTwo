using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;

//NOTE: THIS CLASS RELIES ON PLUMBASESCRIPTS
//v is a scene-dependancy!
namespace BaseUI{
    //v this singleton manages the playersettings
    public class BUI_PlayerSettings : Singleton<BUI_PlayerSettings>
    {
        public struct PlayerSettings{
            public float volume;
            public int qualityIndex;
            public int resolutionIndex;
            public bool fullScreen;
        }
        private PlayerSettings settings = new PlayerSettings();
        public PlayerSettings GetSettings(){
            return settings;
        }
        private PlayerSettings Default(){
            PlayerSettings settingslcl = new PlayerSettings();

            settingslcl.volume = .5f;
            settingslcl.qualityIndex = 0;
            settingslcl.fullScreen = true;
            settingslcl.resolutionIndex = Screen.resolutions.Length - 1;
            return settingslcl;
        }
        private const string dataKey = "Base_PlayerSettings", folderName = "Settings";
        //v load settings on start
        protected override void Awake(){
            base.Awake();
            settings = Default();
            settings = SavingSystem<PlayerSettings>.LoadData(settings, folderName, dataKey, out bool success);
            ApplySettings();
        }

        //v Apply all settings
        public void ApplySettings(){
            ApplyAudioSettings();
            ApplyQualitySettings();
            ApplyFullscreen();
        }

        //v save the data!
        public void SaveSettings(){
            SavingSystem<PlayerSettings>.SaveThisToJson(settings, folderName, dataKey);
        }


#region SPECIFICS
//v Audio Save & Update
        public void ApplyAudioSettings(){
            AudioListener.volume = settings.volume;
        }

        public void ApplyFullscreen(){
            Screen.fullScreen = settings.fullScreen;
        }

        public void ApplyQualitySettings(){
            QualitySettings.SetQualityLevel(settings.qualityIndex);
        }

        public void ApplyResolutionSettings()
        {
            Debug.Log(settings.resolutionIndex + "  i");
            Resolution r = Screen.resolutions[settings.resolutionIndex];
            Screen.SetResolution(r.width, r.height, settings.fullScreen);
        }

        public void UpdateAudioSettings(float p){
            settings.volume = p;
            ApplyAudioSettings();
        }

        public void UpdateFullScreenSettings(bool b){
            settings.fullScreen = b;
            ApplyFullscreen();
        }

        public void UpdateQualitySettings(int index){
            settings.qualityIndex = index;
            ApplyQualitySettings();
        }

        public void UpdateResolutionSettings(int i)
        {
            //v inverted so highest is at the top
            int amt = Screen.resolutions.Length - i - 1;
            settings.resolutionIndex = amt;
            ApplyResolutionSettings();
        }
#endregion
    }

}
