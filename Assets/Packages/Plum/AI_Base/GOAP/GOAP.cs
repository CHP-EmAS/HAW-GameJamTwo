using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plum.Base;
using Plum.AI;

namespace Plum.AI.GOAP
{
    //Based on: https://www.youtube.com/watch?v=gm7K68663rA&t=845s
    //GOAP is also a state-machine
    public abstract class GOAP : MonoBehaviour
    {
        protected struct Goal{
            public State desiredState;
        }
        protected State currentState;
        protected State[] currentPlan;

        protected abstract class State : AStarPathnode{
            public override int GetHCost()
            {
                return hCost;
            }
            protected abstract void OnInit();
            protected abstract void Tick();
            protected abstract void OnEnd();

            public void Init(State[] dependencies){
                this.neighbours.AddRange(dependencies);
            }
        }



        protected State[] GetPlanRaw(Goal g){
            State desired = g.desiredState;
            State start = currentState;
            
            //ASTAR. Also this should be a coroutine because performance and stuff <.< or rather Optionally a coroutine :D OR COROUTINE WITH OPTIONAL WAIT INPUT YES THATS SPLENDID UWU
            currentPlan = PathFinding<State>.AStarRaw(start, desired, CalculateGCost).ToArray();
            return currentPlan;
        }

        protected void GetPlanQueued(Goal g, float waitTime){
            State desired = g.desiredState;
            State start = currentState; 
            StartCoroutine(PathFinding<State>.AStar(start, desired, CalculateGCost, ExtractAStar, waitTime));
        }

        protected virtual int CalculateGCost(State a, State b){
            return 1;
        }
        protected virtual void ExtractAStar(List<State> planInput){
            currentPlan = planInput.ToArray();
        }
        protected abstract void InitStates();
        protected abstract bool CheckGoalValidity(Goal g);
    }
}
