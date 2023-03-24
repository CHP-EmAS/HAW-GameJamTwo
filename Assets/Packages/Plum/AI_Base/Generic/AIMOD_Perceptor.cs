using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Plum.Base;

namespace Plum.AI
{
    //Emulates eyes for AI
    public class AIMOD_Perceptor : AIModule
    {
        
        [Header("Values")]
        [SerializeField] private PhysicsSpace space;
        [SerializeField] private float maxPerceptionRange = 5, maxPatrolPointRange = 1;
        [SerializeField, Range(0, 360)] private float viewDegrees = 0;
        [SerializeField] private List<GameObject> ignoreColliders = new List<GameObject>();
        [SerializeField] private List<string> ignoreTags;
        [SerializeField] private LayerMask viewLayers, groundLayers;

        private List<GameObject> visibleObjects = new List<GameObject>();

        protected Vector3 ViewDir(){
            Vector3 fwd = space == PhysicsSpace._3D? transform.forward : transform.right;
            return fwd;
        }

        //v this methods gets called every checkRangeSeconds and gains information about all surrouding gameObjects which lie in view-range
        public List<GameObject> CheckSphere()
        {
            visibleObjects.Clear();
            Component[] colliders;
            if(space == PhysicsSpace._3D){
                colliders = Physics.OverlapSphere(transform.position, maxPerceptionRange, viewLayers);
            } else{
                colliders = Physics2D.OverlapCircleAll(transform.position, maxPerceptionRange, viewLayers);
            }

            foreach(Component c in colliders)
            {
                //if it should be ignored we can just ignore
                if (ignoreColliders.Contains(c.gameObject) || ignoreTags.Contains(c.tag))
                {
                    continue;
                }

                //Check for view-angle
                Vector3 direction = c.transform.position - transform.position;
                
                if(space == PhysicsSpace._2D) direction.z = 0;
                float angle = Vector3.Angle(direction.normalized, ViewDir());
                if(angle > viewDegrees * .5f)
                {
                    //outisde of view range so it can be discarded
                    continue;
                }


                //if hit
                if(space == PhysicsSpace._3D){
                    if(Physics.Raycast(transform.position, direction, out RaycastHit hit, direction.magnitude, groundLayers))
                    {
                        //not visible
                        continue;
                    }
                    //v^ no hit (visible)
                    else
                    {
                        visibleObjects.Add(c.gameObject);
                    }
                } else{
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, groundLayers);
                    if(hit.collider != null)
                    {
                        //not visible
                        continue;
                    }
                    //v^ no hit (visible)
                    else
                    {
                        visibleObjects.Add(c.gameObject);
                    }
                }

            }

            return visibleObjects;
        }

    public Vector3 GetRandomPatrolPoint(){

        float radius = Random.Range(.1f, maxPatrolPointRange);
        float samplePoint = Random.Range(.5f, 3.14159f);

        Vector3 circle = new Vector3(Mathf.Sin(samplePoint), 0.0f, Mathf.Cos(samplePoint)) * radius;

        Vector3 result = transform.position + circle;

        //!get point based on navmesh samplePosition
        if(NavMesh.SamplePosition(result, out NavMeshHit hit, 15, 0)){
            result = hit.position;
        }
        
        return result;
    }

#region DEBUG&MECHANICAL
        #if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool allowDebugDraw = true;
        private void OnDrawGizmosSelected(){
            if(!allowDebugDraw) return;

            //draw max range sphere
            Gizmos.color = new Color(1, 0, 1, .1f);
            Gizmos.DrawSphere(transform.position, maxPerceptionRange);

            Gizmos.color = new Color(1, 0, 0, .4f);

            //Draw vision cone
            //https://stackoverflow.com/questions/52130986/can-we-create-a-gizmos-like-cone-in-unity-with-script
            float halfDeg = viewDegrees * .5f;
            
            Vector3 nDir = Vector3.Cross(ViewDir(), transform.up);
            Quaternion upDir = Quaternion.AngleAxis(halfDeg, nDir);
            Quaternion downDir = Quaternion.AngleAxis(-halfDeg, nDir);

            Quaternion rightDir = Quaternion.AngleAxis(halfDeg, transform.up);
            Quaternion leftDir = Quaternion.AngleAxis(-halfDeg, transform.up);

            Vector3 dirUp =  upDir * ViewDir() * maxPerceptionRange;
            Vector3 dirDown =  downDir * ViewDir() * maxPerceptionRange;

            Vector3 dirRight =  rightDir * ViewDir() * maxPerceptionRange;
            Vector3 dirLeft =  leftDir * ViewDir() * maxPerceptionRange;

            Gizmos.DrawRay(transform.position, dirUp);
            Gizmos.DrawRay(transform.position, dirDown);
            Gizmos.DrawRay(transform.position, dirRight);
            Gizmos.DrawRay(transform.position, dirLeft);
            Gizmos.DrawRay(transform.position, ViewDir() * maxPerceptionRange);



            //Draw all visible gameObjects
            Gizmos.color = Color.red;
            foreach(GameObject g in visibleObjects)
            {
                Gizmos.DrawRay(transform.position, g.transform.position - transform.position);
            }
        }
        #endif

        //Mechanical variables
        private float refTimeCheck;
#endregion
    }
}
