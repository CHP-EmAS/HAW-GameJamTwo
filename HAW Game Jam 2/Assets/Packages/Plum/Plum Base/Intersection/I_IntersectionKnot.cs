using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public interface IIntersectionModule<T>{
        public void Initialize(T parent);
    }

    //v this is basically the intersection-system but
    public class I_IntersectionKnot<T, N> : MonoBehaviour where N : IIntersectionModule<T>
    {
        [SerializeField, SerializeReference] protected N[] modules;
        protected void Initialize(T parent){
            foreach (IIntersectionModule<T> item in modules)
            {
                item.Initialize(parent);
            }
        }
    }
}
