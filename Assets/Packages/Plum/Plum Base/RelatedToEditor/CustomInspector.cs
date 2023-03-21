using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Plum.Base
{
    public class CustomInspector<T> : Editor where T : UnityEngine.Object
    {
        //https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/
        protected T Item{
            get{
                return target as T;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
        }
    }
}
#endif