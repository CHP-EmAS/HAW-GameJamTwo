using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //v Inherit from this class to allow for Pathfinding!
    public abstract class AStarPathnode : MonoBehaviour{
        public List<AStarPathnode> neighbours = new List<AStarPathnode>();
        public bool isValid = true;
        public virtual int GetHCost(){return hCost;}
        public abstract void OnSelected();
        public abstract void OnCalculated();
        public int localFCost;
        protected int hCost;
    }

    public static class PathFinding<T> where T : AStarPathnode
    {
        //https://www.youtube.com/watch?v=PzEWHH2v3TE

#region EXTERN
        public static int CalcGCostWS(AStarPathnode a, AStarPathnode b){
            int dst = Mathf.RoundToInt(Vector3.Distance(a.transform.position, b.transform.position) * 100);
            return dst;
        }
        //v A* pathfinding
        //v in onResult we extract our results to the destination
        public static IEnumerator AStar(T goal, T start, System.Func<T, T, int> calculateGCost, System.Action<List<T>> onResult, float iterationWait){
            int currentCost = 0;
            List<T> openNodes = new List<T>();
            openNodes.Add(start);
            List<T> closedNodes = new List<T>();
            List<T> finalPath = new List<T>();
            Dictionary<T, T> backTrace = new Dictionary<T, T>();        //Note to self: Key, Value


            //main loop
            while(openNodes.Count > 0){
                T node = openNodes[0];          //<- first node is always with lowest FCost (except for IT1)
                node.OnSelected();

                if(node == goal){
                    closedNodes.Add(openNodes[0]);
                    break;
                } 

                bool result = AStarStep(node, goal, ref currentCost, 
                openNodes, closedNodes, backTrace, 
                calculateGCost);

                if(result){
                    onResult?.Invoke(null);
                    yield break;        //<- stop coroutine on failure
                }

                node.OnCalculated();
                if(iterationWait >= 0) yield return new WaitForSeconds(iterationWait);     //<- we time slice this cutipie to save some performance
            }

            //v after everything is done trace the path back and...
            T currentN = goal;
            while(currentN != start){
                T back = backTrace[currentN];
                finalPath.Add(back);
                currentN = back;
            }

            //... 'export' it!
            onResult?.Invoke(finalPath);
        }

        //v same as above just 'singularly methodonized'
        public static List<T> AStarRaw(T goal, T start, System.Func<T, T, int> calculateGCost){
            int currentCost = 0;
            List<T> openNodes = new List<T>();
            openNodes.Add(start);
            List<T> closedNodes = new List<T>();
            List<T> finalPath = new List<T>();
            Dictionary<T, T> backTrace = new Dictionary<T, T>();        //Note to self: Key, Value


            //main loop
            while(openNodes.Count > 0){
                T node = openNodes[0];          //<- first node is always with lowest FCost (except for IT1)
                node.OnSelected();

                if(node == goal){
                    closedNodes.Add(openNodes[0]);
                    break;
                } 

                bool result = AStarStep(node, goal, ref currentCost, 
                openNodes, closedNodes, backTrace, 
                calculateGCost);

                if(result){
                    return null;        //<- stop on failure
                }

                node.OnCalculated();
            }

            T currentN = goal;
            while(currentN != start){
                T back = backTrace[currentN];
                finalPath.Add(back);
                currentN = back;
            }

            return finalPath;
        }


        public static Vector3 GetPathfollowVelocity(Vector3 currentPosition, List<T> nodes, float minThreshhold = .2f){
            if(nodes.Count == 0){
                return Vector3.zero;
            }

            T currentNode = nodes[nodes.Count - 1];
            currentPosition.z = currentNode.transform.position.z;
            Vector3 movement = currentNode.transform.position - currentPosition;
            float currentDistance = movement.magnitude;
            if(currentDistance <= minThreshhold){
                //reached this
                nodes.Remove(currentNode);
                movement.z = 0;
                return movement.normalized;
            } else{
                movement.z = 0;
                return movement.normalized;
            }
            
        }

#endregion

#region INTERN
        //v single A* Iteration
        private static bool AStarStep(T target, T goal, ref int currentCost, 
        List<T> openList, List<T> closedList, Dictionary<T, T> refD,            //<- we pass in lists and dictionaries for reference
        System.Func<T, T, int> calculateGCost){                                 //<- also to stay as abstract as possible, we want to be able to customize our gCost calculation formula
            
            //v loop through all nodes
            foreach(T node in target.neighbours){
                if(closedList.Contains(node)) continue;
                if(!node.isValid){
                    if(!closedList.Contains(node)) closedList.Add(node);
                    continue;
                }

                node.OnCalculated();

                node.localFCost = target.localFCost + node.GetHCost() + calculateGCost(target, node);

                if(!refD.ContainsKey(node)) refD.Add(node, target);
                if(!openList.Contains(node) && !closedList.Contains(node)) openList.Add(node);

                if(node == goal){
                    //v make sure the goal is the next analyzed node to stop the algorithm and safe some performance
                    openList.Clear();
                    openList.Add(node);
                    return false;
                }
            }
            openList.Remove(target);
            closedList.Add(target);

            openList.Sort((x,y) => x.localFCost.CompareTo(y.localFCost));
            if(openList.Count <= 0) return true;

            currentCost = openList[0].localFCost;                   //<- we always start with the lowest FCost
            return false;
        }

    }
#endregion

}
