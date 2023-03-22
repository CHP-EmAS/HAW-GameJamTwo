using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.CustomDebug{
//https://answers.unity.com/questions/125049/is-there-any-way-to-view-the-console-in-a-build.html
 public class ConsoleToGUI : MonoBehaviour
 {
     [SerializeField] private  KeyCode input = KeyCode.F1;
     string myLog = "*begin log";
     string filename = "";
     bool doShow = false;
     int kChars = 700;
     void OnEnable() { Application.logMessageReceived += Log; }
     void OnDisable() { Application.logMessageReceived -= Log; }
     void Update() { if (Input.GetKeyDown(input)) { doShow = !doShow; } }
     public void Log(string logString, string stackTrace, LogType type)
     {
        // for onscreen...
         myLog = myLog + "\n" + logString;
         if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }
     }
 
     void OnGUI()
     {
         if (!doShow) { return; }
         GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
         GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
     }
 }
}
