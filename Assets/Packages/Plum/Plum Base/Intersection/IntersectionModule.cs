using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    //This class represents any module extension for the intersection pattern
    public class IntersectionModule<Parent, Child> : MonoBehaviour 
    where Parent : IntersectionNode<Parent, Child>  
    where Child: IntersectionModule<Parent, Child>
    {
        protected System.Action<Parent> onInit; //<- Utility method
        protected Parent parent;
        public void Initialize(Parent parent){
            this.parent = parent;
            onInit?.Invoke(parent);
        }
    }

}
