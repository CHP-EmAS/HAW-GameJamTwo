using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class EnumBasedStateMachineSimple<T> : MonoBehaviour where T : System.Enum
    {
        protected T currentState;
        protected Utility.ArgumentelessDelegate[] inits, updates, exits;
        private int stateAmount = 0;

        protected virtual void Awake(){
            stateAmount = System.Enum.GetNames(typeof(T)).Length;
            inits = new Utility.ArgumentelessDelegate[stateAmount];
            updates = new Utility.ArgumentelessDelegate[stateAmount];
            exits = new Utility.ArgumentelessDelegate[stateAmount];
        }

        protected int GetIndex(T state){
            return System.Convert.ToInt32(state);
        }

        protected virtual void Tick(){
            updates[GetIndex(currentState)].Invoke();
        }

        protected void ChangeState(T newState){
            inits[GetIndex(newState)].Invoke();
            exits[GetIndex(newState)].Invoke();
        }

#region SUBSCRIPTIONS
        private void Subscribe(Utility.ArgumentelessDelegate del, Utility.ArgumentelessDelegate target){
            del += target;
        }

        public void SubscribeOnInit(T targetState, Utility.ArgumentelessDelegate target){
            Subscribe(inits[GetIndex(targetState)], target);
        }

        public void SubscribeOnUpdate(T targetState, Utility.ArgumentelessDelegate target){
            Subscribe(updates[GetIndex(targetState)], target);
        }

        public void SubscribeOnExit(T targetState, Utility.ArgumentelessDelegate target){
            Subscribe(exits[GetIndex(targetState)], target);
        }
#endregion
    }
}
