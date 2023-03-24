using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Plum.AI;

namespace Music.Combat
{
    public class EnemyBrain : AI_Brain, IMoveable
    {
        public void Move(Vector3 a)
        {

        }
        public void AddForce(Vector3 v, bool sh = false)
        {
            agent.velocity += v * .1f;
        }
        private const float tickTime = .1f;
        private const string playerTag = "Player";
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private AIMOD_Perceptor perceptor;
        protected override void GoTo(Vector3 point)
        {
            agent.SetDestination(point);
        }

        private bool Evaluate()
        {
            List<GameObject> selection = perceptor.CheckSphere();
            bool didSee = false;
            foreach(GameObject seen in selection)
            {
                if (seen.gameObject.CompareTag(playerTag)){
                    ChangeToWander(seen.gameObject.transform.position);
                    didSee = true;
                }
            }
            return didSee;
        }

        private float evaluateTimer = 0.0f;
        private bool DoEvaluate()
        {
            if(evaluateTimer >= 0)
            {
                evaluateTimer -= Time.deltaTime;
                return true;
            }
            else
            {
                evaluateTimer = tickTime;
                return Evaluate();
            }
        }

        private void ChangeToWander(Vector3 pos)
        {
            GoTo(pos);
            ChangeState(AIState.WANDER);
        }

        protected override void Stent_Idle()
        {
            agent.speed = 1.0f;
            GoTo(perceptor.GetRandomPatrolPoint());
        }

        private const float minPointThreshhold = 1.0f;
        protected override void Stupt_Idle()
        {
            if(Vector3.Distance(transform.position, agent.destination) <= minPointThreshhold)
            {
                GoTo(perceptor.GetRandomPatrolPoint());
            }
            DoEvaluate();
        }

        protected override void Stext_Idle()
        {
            
        }

        protected override void Stent_Wander()
        {
            agent.speed = 3.5f;
        }

        protected override void Stupt_Wander()
        {
            if(!DoEvaluate()){
                ChangeState(AIState.IDLE);
            }
        }

        protected override void Stext_Wander()
        {
        }
    }

}
