using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Plum.Base;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BaseUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class BUI_Mesh : UIBehaviour
    {
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private bool updateScale, updateOnDimensionChange = false; public bool UpdateScale { get => updateScale; }
        [HideInInspector] private float overAllScale = .01f; public float Scale { set => overAllScale = value; get => overAllScale; }
        private CanvasRenderer r;
        private RectTransform rect;

        [ContextMenu("Manual Update")]
        protected override void Start(){
            base.Start();
            rect = GetComponent<RectTransform>();
            r = GetComponent<CanvasRenderer>();
            UpdateMesh();
        }

        private void UpdateMesh(){
            r.Clear();
            r.SetMaterial(material, null);
            r.SetMesh(GetNewMesh());
        }

        private void ClearMesh(){
            r.Clear();
        }


        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if(!updateOnDimensionChange) return;
            if(updateScale){
                UpdateMesh();
            }
        }


        private Mesh GetNewMesh(){
            Mesh m = Instantiate(mesh);

            if(!updateScale) return m;
            List<Vector3> positions = new List<Vector3>();
            positions.AddRange(m.vertices);
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] = new Vector3(positions[i].x * rect.sizeDelta.x * 50, positions[i].y * rect.sizeDelta.y * 50, positions[i].z * rect.sizeDelta.x * 50) * overAllScale;
            }
            m.SetVertices(positions);
            return m;
        }


        protected override void OnDisable(){
            base.OnDisable();
            ClearMesh();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(rect == null) rect = GetComponent<RectTransform>();
            if(r == null) r = GetComponent<CanvasRenderer>();
            UpdateMesh();
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BUI_Mesh))]
    [CanEditMultipleObjects]
    public class BUIMeshEditor : CustomInspector<BUI_Mesh>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Item.UpdateScale)
            {
                Item.Scale = EditorGUILayout.FloatField(Item.Scale);
            }
        }
    }
#endif
}
