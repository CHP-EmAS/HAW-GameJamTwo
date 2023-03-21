using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class DynamicPool<T>
    {
        private int maxAmount = -1;
        public DynamicPool(){}
        public DynamicPool(int maxAmount){
            this.maxAmount = maxAmount;
        }


        private List<T> unused = new List<T>(), inUsage = new List<T>();
        public T[] GetUnused() => unused.ToArray();
        public T[] GetInusage() => inUsage.ToArray();

        public bool HasUnusedInstances() { return unused.Count > 0; }
        public bool AllowMoreInstances(){
            if(maxAmount <= 0) return true;
            else return inUsage.Count < maxAmount;
        }

        public bool avoidParent = false;
        private GameObject parent;
        private void Parent(MonoBehaviour reference){
            if(avoidParent) return;
            if(parent == null){
                parent = new GameObject();
            }

            parent.name = reference.GetType() + " - dynamicPoolparent";
            reference.transform.parent = parent.transform;
        }

        public void RequestInstance(Utility.GenericDelegate<T> onInstanceTaken, Utility.ArgumentelessDelegate createNewInstance){
            if(HasUnusedInstances()){
                onInstanceTaken?.Invoke(GetUnusedInstance());
            } else{
                if(AllowMoreInstances()){
                    createNewInstance?.Invoke();
                }
                else{
                    onInstanceTaken?.Invoke(GetInUseInstance());
                }
            }
        }
        public void RequestInstanceAddAuto(Utility.GenericDelegate<T> onInstanceTaken, System.Func<T> createNewInstance){
            if(HasUnusedInstances()){
                onInstanceTaken?.Invoke(GetUnusedInstance());
            } else{
                if(AllowMoreInstances()){
                    T instance = createNewInstance();
                    
                    if(instance is MonoBehaviour){
                        Parent(instance as MonoBehaviour);
                    }

                    inUsage.Add(instance);
                }
                else{
                    onInstanceTaken?.Invoke(GetInUseInstance());
                }
            }
        }

        public void AddUnusedInstances(List<T> Ts)
        {
            unused.AddRange(Ts);
        }

        public void AddInUseInstance(T instance){
            if(instance is MonoBehaviour){
                Parent(instance as MonoBehaviour);
            }

            inUsage.Add(instance);
        }

        public void Regress(){
            unused.AddRange(inUsage);
            inUsage.Clear();
        }

        public T GetUnusedInstance()
        {
            T p = unused[0];
            unused.Remove(p);
            inUsage.Add(p);
            return p;
        }

        public T GetInUseInstance(){
            T p = inUsage[0];
            inUsage.Remove(p);
            inUsage.Add(p);         //<- inserting at the other end
            return p;
        }
    }
}
