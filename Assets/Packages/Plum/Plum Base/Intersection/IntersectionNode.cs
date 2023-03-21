using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //This class is the node for the module pattern
    public class IntersectionNode<Parent, Child> : MonoBehaviour 
    where Parent : IntersectionNode<Parent, Child>  
    where Child: IntersectionModule<Parent, Child>
    {
        protected class GenericKey<T> where T : Child{}
        protected List<Child> children = new List<Child>();    //<- child array, set automatically.
        protected System.Action<Child> onChildInit; //<- Utility action
        protected Dictionary<System.Type, Child> references = new Dictionary<System.Type, Child>();

        //we may easily add children in Awake so having this thingy in start is good
        protected virtual void Start(){
            Child[] childrenLCL = GetComponentsInChildren<Child>();
            children.AddRange(childrenLCL);

            for (int i = 0; i < children.Count; i++)
            {
                Child child = children[i];
                InitChild(child);
            }
        }

        protected virtual void InitChild(Child child){
            child.Initialize(this as Parent);
            onChildInit?.Invoke(child);
            if(!references.ContainsKey(child.GetType()))references.Add(child.GetType(), child);
        }
    }

}
