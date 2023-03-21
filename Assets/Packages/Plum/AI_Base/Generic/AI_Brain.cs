using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Plum.Base;

namespace Plum.AI
{
    public enum AIState{
        IDLE = 0,
        WANDER = 1,
        STATIC = 2,
    }
    public abstract class AI_Brain : IntersectedStateMachineSimple<AIState, AI_Brain, AIModule>
    {
    [SerializeField, Header("Abstract Brain variables")] protected AI_BrainSCROB brainDesc;
    [SerializeField] private AIState defaultState;
    public AI_BrainSCROB Description{get{return brainDesc;}}
    [SerializeField] private bool avoidDebugGizmos = false;

    protected Vector3 targetPosition;

    protected override void Start(){
        base.Start();
        targetPosition = transform.position;
        SubscribeOnInit(AIState.IDLE, Stent_Idle);
        SubscribeOnInit(AIState.WANDER, Stent_Wander);
        SubscribeOnInit(AIState.STATIC, Stent_Static);

        SubscribeOnUpdate(AIState.IDLE, Stupt_Idle);
        SubscribeOnUpdate(AIState.WANDER, Stupt_Wander);
        SubscribeOnUpdate(AIState.STATIC, Stupt_Static);

        SubscribeOnExit(AIState.IDLE, Stext_Idle);
        SubscribeOnExit(AIState.WANDER, Stext_Wander);
        SubscribeOnExit(AIState.STATIC, Stext_Static);

        ChangeState(defaultState);
    }

    protected virtual void Update(){
        Tick();     //<- we update state-behaviours
    }

#region INITS
    protected virtual void Stent_Idle(){
        
    }
    protected virtual void Stent_Wander(){        
        GoTo(targetPosition);
    }
    protected virtual void Stent_Static(){

    }
#endregion
#region UPDATES
    protected virtual void Stupt_Idle(){}
    
    protected virtual void Stupt_Wander(){
        GoTo(targetPosition);
    }

    protected virtual void Stupt_Static(){
        
    }
#endregion
#region EXITS
    protected virtual void Stext_Idle(){}
    protected virtual void Stext_Wander(){}
    protected virtual void Stext_Static(){}
#endregion  

    protected abstract void GoTo(Vector3 point);


    private void OnDrawGizmosSelected(){
        if(avoidDebugGizmos) return;
        //draw max range sphere
        Gizmos.color = new Color(1, 1, 0, .2f);
        Gizmos.DrawSphere(targetPosition, 1.0f);
    }
}

}
