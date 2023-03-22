using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Plum.Devtools{
public abstract class CustomDebugConsole : MonoBehaviour
{
    protected const KeyCode inputCode = KeyCode.F2;
    public void SetConsole(bool status){
        showConsole = status;
    }
    protected bool showConsole = false;
    public string commandString = "Write here!";
    protected bool allowThis = false;

    protected virtual void Update(){
        if(!allowThis) return;
        if(Input.GetKeyDown(inputCode)){
            showConsole = !showConsole;
        }

        if(showConsole){
                if(Input.GetKeyDown(KeyCode.Return)){
                ExecuteCommand(commandString);
                showConsole = false;
                commandString = "";
            }
        }
    }


    private void OnGUI(){
        if(!showConsole) return;
        float y = 0;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        commandString = GUI.TextField(new Rect(10f, y + 5, Screen.width-20f, 20f), commandString);

        //v go through executed commands
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            currentLastCommandIndex++;
            currentLastCommandIndex = Mathf.Clamp(currentLastCommandIndex, 0, lastcommands.Count);
            commandString = lastcommands[currentLastCommandIndex];
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)){
            currentLastCommandIndex--;
            currentLastCommandIndex = Mathf.Clamp(currentLastCommandIndex, 0, lastcommands.Count);
            commandString = lastcommands[currentLastCommandIndex];
        }
    }

    private void ExecuteCommand(string str){
        Debug.Log("executing:- " + str); 
        CommandDefinitions(str);
        showConsole = false;
    }

    protected List<string> lastcommands = new List<string>();
    protected int currentLastCommandIndex = 0;
    protected virtual void CommandDefinitions(string str){
        lastcommands.Add(str);
        currentLastCommandIndex = 0;
    }
}

}
