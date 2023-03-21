using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public abstract class SCROBType<T> : ScriptableObject
    {
        public T toAccess;
    }
}
