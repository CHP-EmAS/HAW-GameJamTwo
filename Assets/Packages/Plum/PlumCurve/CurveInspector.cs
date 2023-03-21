using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Plum.Base;

namespace Plum.Curve
{

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DynamicCurve))]
    public class CurveInspector : CustomPropertyDrawer<DynamicCurve>
    {        
        private enum ResponseType{
            STEP = 0,
            SIN = 1
        }
        private const int propertyHeightAdd = 23;
        ResponseType responseType = ResponseType.STEP;
        bool animate = false;

#region UTILS
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2);

            int lineAMT = 1;

            if(property.isExpanded){
                lineAMT = propertyHeightAdd;
            }

            return height * lineAMT;
        }

        private Rect GetDownPosition(Rect pivot, ref int downCounter, float inoff, ref float offset){
            Rect newRect = pivot;

            newRect.y += downCounter * EditorGUIUtility.singleLineHeight + offset + inoff;
            newRect.height = EditorGUIUtility.singleLineHeight;
            offset += inoff;

            downCounter++;
            return newRect;
        }

        private Vector3 GetRectAsPosition(Rect r){
            return new Vector3(r.x, r.y, 0.0f);
        }
#endregion

        private int graphResolution = 50;
        private float graphMaxValue = 10;
        public override async void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
#region BASE
            float addOffset = 0;
            EditorGUI.BeginProperty(position, label, property);
            {
                Rect foldOut = new Rect(position.min.x, position.min.y, position.size.x, 
                EditorGUIUtility.singleLineHeight);

                property.isExpanded = EditorGUI.Foldout(foldOut, property.isExpanded, label);
            }
            EditorGUI.EndProperty();



            if(!property.isExpanded) return;

            int downCounter = 1;
            Rect GetDownRect(float additionalOffset){
                return GetDownPosition(position, ref downCounter, additionalOffset, ref addOffset);
            }

#endregion


            float m = 1.0f;
            float k = .4f;

            var typeVal = property.FindPropertyRelative("equationType");
            EquationType type = (EquationType)typeVal.intValue;

            EditorGUI.BeginProperty(position, label, property);

            type = (EquationType)EditorGUI.EnumPopup(GetDownRect(0), type);
            typeVal.enumValueIndex = (int)type;

            var a0 = property.FindPropertyRelative("a0");
            var a1 = property.FindPropertyRelative("a1");
            var a2 = property.FindPropertyRelative("a2");
            var inten = property.FindPropertyRelative("intensity");
            var spd = property.FindPropertyRelative("speed");

            switch(type){
                case EquationType.SPRING:
                EditorGUI.LabelField(GetDownRect(0), "Speed");
                spd.floatValue = EditorGUI.Slider(GetDownRect(0), spd.floatValue, 0.0f, 10);

                EditorGUI.LabelField(GetDownRect(0), "A0");
                a0.floatValue = EditorGUI.Slider(GetDownRect(0), a0.floatValue, 0.0f, 5);

                EditorGUI.LabelField(GetDownRect(0), "A1");
                a1.floatValue = EditorGUI.Slider(GetDownRect(0), a1.floatValue, 0.0f, 5);

                EditorGUI.LabelField(GetDownRect(0), "A2");
                a2.floatValue = EditorGUI.Slider(GetDownRect(0), a2.floatValue, -5.0f, 5);

                EditorGUI.LabelField(GetDownRect(0), "Max Speed");
                inten.floatValue = EditorGUI.FloatField(GetDownRect(0), inten.floatValue);

                m = a1.floatValue;
                k = a0.floatValue;
                break;

                case EquationType.SMOOTH:
                EditorGUI.LabelField(GetDownRect(0), "Speed");
                spd.floatValue = EditorGUI.Slider(GetDownRect(0), spd.floatValue, .0001f, 10);

                EditorGUI.LabelField(GetDownRect(0), "BaseStiffness");
                a0.floatValue = EditorGUI.Slider(GetDownRect(0), a0.floatValue, .0001f, 2.0f);

                EditorGUI.LabelField(GetDownRect(0), "Smoothness");
                a1.floatValue = EditorGUI.Slider(GetDownRect(0), a1.floatValue, 0.0001f, 2);

                //EditorGUI.LabelField(GetDownRect(0), "a3");
                //at.floatValue = EditorGUI.Slider(GetDownRect(0), at.floatValue, .0001f, 2);
                
                EditorGUI.LabelField(GetDownRect(0), "Max Speed");
                inten.floatValue = EditorGUI.FloatField(GetDownRect(0), inten.floatValue);

                m = a1.floatValue;
                k = a0.floatValue;
                break;
            }
            EditorGUI.EndProperty();

            //v draw graph
            Rect grid = GetDownRect(10);
            addOffset += grid.height * 3;
            grid.height *= 3.0f;
            float InitialYResponse(float x, out float T){
                switch (responseType)
                {
                    case ResponseType.STEP:
                        T = x >= grid.max.x * .5f? 1.0f : 0.0f;
                        break;

                    case ResponseType.SIN:
                        T = Mathf.Sin(x * .1f) + 1.0f;
                        T *= .5f;
                        break;

                    default:
                        T = 0;
                        break;
                }
                return Mathf.Lerp(grid.max.y, grid.min.y, T);
            }


            Vector3 velocity = Vector3.zero;
            Vector3 lt = Vector3.zero;
            float currentPos = grid.max.y;

            DynamicCurve curve = new DynamicCurve();
            curve.a0 = a0.floatValue;
            curve.a1 = a1.floatValue;
            curve.a2 = a2.floatValue;
            curve.speed = spd.floatValue;
            curve.intensity = 999.999f;
            lt.x = currentPos;

            float dt = spd.floatValue / (float)(graphResolution + .0001f);

            Vector3 ResponseGraphVertex(float x, float y){
                y = InitialYResponse(x, out float T);

                currentPos = DynamicCurve.GetFunc(type)(currentPos, y, ref curve, ref velocity.x, ref lt.x, dt);    //<- avrg deltatime on my system

                return new Vector3(x, currentPos, 0);
            }

            //v draw base response graph
            Vector3 GraphVertex(float x, float y){
                y = InitialYResponse(x, out float N);
                Vector3 position = new Vector3(x, y, 0);
                return position;
            }

            EditorUtilsPlum.DrawGUIGraph(grid, .5f, Color.red, graphResolution, GraphVertex);
            EditorUtilsPlum.DrawGUIGraph(grid, 0.0f, Color.blue, graphResolution, ResponseGraphVertex);

            EditorGUI.BeginProperty(position, label, property);
            {
                responseType  = (ResponseType)EditorGUI.EnumPopup(GetDownRect(0), responseType);

                EditorGUI.LabelField(GetDownRect(0), "Graph Rez.");
                graphResolution = EditorGUI.IntSlider(GetDownRect(0), graphResolution, 3, 100);
                //EditorGUI.LabelField(GetDownRect(0), "Graph Max Value.");
                //graphMaxValue = EditorGUI.Slider(GetDownRect(0), graphMaxValue, 1, 500);


                EditorGUI.LabelField(GetDownRect(0), "Animate");
                animate = EditorGUI.Toggle(GetDownRect(0), animate);
            }
            EditorGUI.EndProperty();


            if(!animate) return;
            //v draw circle
            float sinMappedTo01 = Mathf.Sin(Time.realtimeSinceStartup) + 1;
            sinMappedTo01 *= .5f; 

            float targetX = Mathf.Lerp(grid.min.x, grid.max.x, sinMappedTo01);
            float targetY = InitialYResponse(targetX, out float N);

            currentCirclePosition = new Vector3(targetX, 
            DynamicCurve.GetFunc(type)(currentCirclePosition.y, targetY, ref curve, ref velocity.y, ref lt.y, 1.0f)
            ,0.0f);

            float circleSize = 2;
            EditorUtilsPlum.DrawGUICircle(new Rect(currentCirclePosition.x, currentCirclePosition.y, circleSize, circleSize), Color.red, 12.0f);
        }

        private Vector3 currentCirclePosition = new Vector3(500, 500, 500);

    }
#endif
}
