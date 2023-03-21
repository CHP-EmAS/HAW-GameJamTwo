using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Plum.Base;

//v in contrast to Controls this handles input through delegate subscriptions
public partial class InputHandler : Singleton<InputHandler>
{
    //v all conditions to allow input
    public List<System.Func<bool>> workCondition = new List<System.Func<bool>>(); public void AddWorkCondition(System.Func<bool> cond) => workCondition.Add(cond);
    public void RemoveWorkCondition(System.Func<bool> cond) => workCondition.Remove(cond); public void ClearWorkConditions() => workCondition.Clear();

    public Utility.ArgumentelessDelegate OnPaused;      //<- pause menu is BUI feature
    public Utility.Vector2Delegate mousePosition;       //<- mousePosition is pretty universal
    private bool isAllowed = true;
    public bool IsAllowed { get => isAllowed;}

    //v pause menu implementation. It needs to be assigned either via PlumControls or via a Control-Asset! (Syntax for control asset: X.Y.Performed += _ => Method())
    private void PerformPaused(){
        if(isAllowed) OnPaused?.Invoke();
    }

    //UTILS
    private void Set(bool dir){
        isAllowed = dir;
    }
    public void AllowInput(){
        Set(true);
    }
    public void DisableInput(){
        Set(false);
    }

    private bool StopCondition()
    {
        return !isAllowed || TimeManager.IsPaused || !Continue();
    }

    //HANDLING
    partial void UpdateInput();
    //v basic handlers
    private void PerformMethod(Utility.ArgumentelessDelegate action){
        if(StopCondition()) return;
        action?.Invoke();
    }

    private void PerformUpDownMethods(System.Func<bool> wasReleasedThisFrame, Utility.ArgumentelessDelegate onDown, Utility.ArgumentelessDelegate onUp){
        if(wasReleasedThisFrame()){
            PerformMethod(onUp);
        } else{
            PerformMethod(onDown);
        }
    }

    private void PerformMethod(Utility.BoolDelegate action, bool args){
        if(StopCondition()) return;
        action?.Invoke(args);
    }
    private void PerformMethod(Utility.Vector2Delegate action, Vector2 args){
        if(StopCondition()) return;
        action?.Invoke(args);
    }

    public bool Continue(){
        bool update = true;
        foreach (System.Func<bool> func in workCondition)
        {
            update = update && func();
        }
        return update;
    }


    private void Update(){        
        //v is update allowed?
        if(!Continue()) return;

        if(StopCondition())
        {
            mousePosition?.Invoke(Vector2.zero);
            return;
        } 

        mousePosition?.Invoke(Controls.MousePosition());
        UpdateInput();
    } 
        
}
