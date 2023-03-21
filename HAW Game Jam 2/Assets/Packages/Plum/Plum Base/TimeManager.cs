using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class TimeManager : Singleton<TimeManager>
    {
        public static Utility.ArgumentelessDelegate onUnpaused, onPaused;
        private static float rv0;
        private static float currentTime = 1.0f;
        private static float holdTime = .1f;
        public static void HoldFrame(float lowSpeed, float changeTime){
            holdTime = changeTime;
            currentTime = lowSpeed;
            rv0 = 0;
            SetTimeRaw(currentTime);
        }
        private void Update(){
            if(!isPaused){
                if(currentTime != 1.0f){
                    currentTime = Mathf.SmoothDamp(currentTime, 1.0f, ref rv0, holdTime);
                    SetTimeRaw(currentTime);
                } else{
                    if(Time.timeScale != 1){
                        SetTimeRaw(1.0f);
                    }
                }
            }
        }
        private static float lastTimescale;
        private static bool isPaused = false;
        public static bool IsPaused { get => isPaused; set => isPaused = value; }

        public static void SetTimeRaw(float newTimeScale){  //Like SetTime but for this script 
            Time.timeScale = newTimeScale;
            Time.fixedDeltaTime = ProjectSettings.fixedTimeStep * Time.timeScale;
        }
        public static void SetTime(float newTimeScale){
            if(!isPaused){
                SetTimeRaw(newTimeScale);
            }
        }

        public static void PauseOrContinue(bool pauseOrContinue){ //This should currently be only triggered through GameUIManager
            if(pauseOrContinue) PauseGame();
            else ResumeGame();
        }

        public static void PauseSwitch(){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
        public static void PauseGame(){
            if(!isPaused){
                lastTimescale = Time.timeScale;
                SetTimeRaw(0);
                isPaused = true;
                onPaused?.Invoke();
            }
            else{
                Debug.Log("tried to pause game while it was paused");
            }
        }

        public static void ResumeGame(){
            if(isPaused){
                SetTimeRaw(lastTimescale); 
                onUnpaused?.Invoke();
                isPaused = false;
            }
            else{
                Debug.Log("tried to resume game while it was not paused");
            }
        }

    }

}
