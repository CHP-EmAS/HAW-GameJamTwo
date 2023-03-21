using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Plum.AI;

namespace Music.Combat
{
    public class EnemyBrain : AI_Brain
    {
        private const float tickTime = .1f;
        private const string playerTag = "Player";
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private AIMOD_Perceptor perceptor;
        protected override void GoTo(Vector3 point)
        {
            agent.SetDestination(point);
        }

        private void Evaluate()
        {
            List<GameObject> selection = perceptor.CheckSphere();
            foreach(GameObject seen in selection)
            {
                if (seen.gameObject.CompareTag(playerTag)){
                    ChangeToWander(seen.gameObject.transform.position);
                }
            }
        }

        private float evaluateTimer = 0.0f;
        private void DoEvaluate()
        {
            if(evaluateTimer > 0)
            {
                evaluateTimer -= Time.deltaTime;
            }
            else
            {
                Evaluate();
                evaluateTimer = tickTime;
            }
        }

        private void ChangeToWander(Vector3 pos)
        {
            GoTo(pos);
            ChangeState(AIState.WANDER);
        }

        protected override void Stent_Idle()
        {
            
        }

        protected override void Stupt_Idle()
        {
            DoEvaluate();
        }

        protected override void Stext_Idle()
        {
            
        }

        protected override void Stent_Wander()
        {
        }

        protected override void Stupt_Wander()
        {
            DoEvaluate();
        }

        protected override void Stext_Wander()
        {
        }
    }

}
