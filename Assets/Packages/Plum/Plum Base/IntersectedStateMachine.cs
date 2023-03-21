using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public abstract class IntersectedStateMachine<T, N, M> : IntersectionNode<N, M> where T : System.Enum 
    where M : IntersectionModule<N, M>
    where N : IntersectionNode<N, M>
    {
        //v abstract struct to represent individual states
        protected struct State{
            public System.Action update;                //<- this structs logic update
            public System.Action onEnter;               //<- method which gets called on enter
            public System.Action onExit;                //<- method which gets called on exit
            public System.Func<bool> enterCondition;    //<- condition to enter this state
        }

        //v current state
        protected T currentState;

        //v all states
        protected State[] states;
        protected virtual void Awake(){
            int amount = System.Enum.GetNames(typeof(T)).Length;
            states = new State[amount];

            for (int i = 0; i < states.Length; i++)
            {
                //v fill in states
                states[i] = FillState((T)System.Enum.ToObject(typeof(T), i));
            }
        }
        
        protected void Update(){
            //v update the current state
            states[System.Convert.ToInt32(currentState)].update();
        }

        protected void CheckForChange(T stateForCheck){
            int newIndex = System.Convert.ToInt32(stateForCheck);
            if(states[newIndex].enterCondition()){
                ChangeState(stateForCheck);
            }
        }

        //v Use this to change states!:)
        protected void ChangeState(T newState){
            int newIndex = System.Convert.ToInt32(newState);
            int oldindex = System.Convert.ToInt32(currentState);

            //v tried to change state to itself - can be ignored
            if(newIndex == oldindex){
                return;
            }

            //v everything after this runs under newindex != oldIndex
            states[oldindex].onExit?.Invoke();
            states[newIndex].onEnter?.Invoke();
        }

        //v use this method to fill in all states!
        protected abstract State FillState(T state);
    }

}
