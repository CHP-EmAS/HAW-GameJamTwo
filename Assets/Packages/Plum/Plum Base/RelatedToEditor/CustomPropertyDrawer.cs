using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Plum.Base
{
    public class CustomPropertyDrawer<T> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }
}
#endif