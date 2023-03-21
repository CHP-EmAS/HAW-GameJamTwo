using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public abstract class IntersectedStateMachineSimple<T, N, M> : IntersectionNode<N, M> where T : System.Enum 
    where M : IntersectionModule<N, M>
    where N : IntersectionNode<N, M>
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
            int toReturn = System.Convert.ToInt32(state);
            return toReturn;
        }

        protected virtual void Tick(){
            updates[GetIndex(currentState)]?.Invoke();
        }

        protected void ChangeState(T newState){
            exits[GetIndex(newState)]?.Invoke();
            inits[GetIndex(newState)]?.Invoke();
            currentState = newState;
        }

#region SUBSCRIPTIONS
        private void Subscribe(Utility.ArgumentelessDelegate del, Utility.ArgumentelessDelegate target){
            del += target;
        }

        public void SubscribeOnInit(T targetState, Utility.ArgumentelessDelegate target){
            inits[GetIndex(targetState)] += target;
        }

        public void SubscribeOnUpdate(T targetState, Utility.ArgumentelessDelegate target){
            updates[GetIndex(targetState)] += target;
        }

        public void SubscribeOnExit(T targetState, Utility.ArgumentelessDelegate target){
            exits[GetIndex(targetState)] += target;
        }
#endregion
    }
}
