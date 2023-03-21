using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Plum.Base
{
    public static class EditorUtilsPlum
    {
        public static void DrawRawGUIGraph(Rect rect, float backGroundOpacity, Color graph, params Vector3[] inBetweens){

            if(rect != null) EditorGUI.DrawRect(rect, new Color(backGroundOpacity, backGroundOpacity, backGroundOpacity, backGroundOpacity));
            Handles.color = graph;

            List<Vector3> data = new List<Vector3>();

            Handles.DrawAAPolyLine(5f, inBetweens);
        }

        public static void DrawGUIGraph(Rect rect, float backGroundOpacity, Color graph, int vertexAmount, System.Func<float, float, Vector3> onVertex){

            List<Vector3> positions = new List<Vector3>();
            for (int v = 0; v < vertexAmount; v++)
            {
                float currentX = Mathf.Lerp(rect.min.x, rect.max.x, (float)v / (float)vertexAmount);
                float currentY = Mathf.Lerp(rect.max.y, rect.min.y, (float)v / (float)vertexAmount);

                Vector3 position = onVertex(currentX, currentY);
                positions.Add(position);
            }     
            DrawRawGUIGraph(rect, backGroundOpacity, graph, positions.ToArray());
        }


        public static void DrawGUICircle(Rect rect, Color color, float radius){
            Handles.color = color;

            Vector3 pos = new Vector3(rect.position.x, rect.position.y, 0.0f);
            Handles.DrawSolidDisc(pos, Vector3.forward, radius);
        }
        

        public static Vector3 GetRectMin(this Rect rect){
            return new Vector3(rect.min.x, rect.max.y);
        }

        public static Vector3 GetRectMax(this Rect rect){
            return new Vector3(rect.max.x, rect.min.y);
        }
    }
}
#endif