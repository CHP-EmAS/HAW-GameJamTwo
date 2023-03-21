using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base.Demos{
    //Use this class as a guide on how to implement the pathfinding!
    public class PathFindingDemo : MonoBehaviour
    {
        public class Tile : AStarPathnode
        {
            public Vector2Int index;
            private SpriteRenderer renderer;
            public Tile(SpriteRenderer renderer, int hCost){
                this.renderer = renderer;
                this.hCost = hCost;
            }

            public void SetColor(Color c){
                renderer.color = c;
            }

            public override void OnCalculated()
            {
                renderer.color = Color.magenta;
            }

            public override void OnSelected()
            {
                renderer.color = Color.green;
            }
        }
        [SerializeField] private Sprite worldSprite;
        [SerializeField] private Vector2Int current, target;
        [SerializeField, Range(0, 100)] private int obstacleChance = 20;
        private const uint dimensionX = 10, dimensionY = 10;
        private List<Tile> tiles = new List<Tile>();
        private Tile[,] tiled2D;
        private void Start(){
            InitializeGrid();
            UpdateCT();
        }

        private void InitializeGrid(){
            //v first make them actually appear
            tiled2D = new Tile[dimensionX, dimensionY];
            for (int x = 0; x < dimensionX; x++)
            {
                for (int y = 0; y < dimensionY; y++)
                {
                    GameObject g = new GameObject();
                    g.transform.position = transform.position + new Vector3(x, y, 0);
                    g.transform.parent = transform;
                    g.name = "Tile: " + x + "|" + y;
                    SpriteRenderer s = g.AddComponent<SpriteRenderer>();
                    s.sprite = worldSprite;

                    Tile t = new Tile(s, 1);
                    t.index = new Vector2Int(x, y);

                    tiles.Add(t);
                    tiled2D[x, y] = t;
                    if((x == current.y && y == current.y) || (x == target.x && (y == target.y))) continue;
                    t.isValid = Utility.Chance(obstacleChance)? false : true;
                }
            }

            for (int x = 0; x < dimensionX; x++)
            {
                for (int y = 0; y < dimensionY; y++)
                {
                    Vector2Int[] neighbours = GenericUtility<Tile>.NeighbouringIndices2DNineFull(tiled2D, new Vector2Int(x, y));
                    foreach (Vector2Int n in neighbours)
                    {
                        tiled2D[x, y].neighbours.Add(tiled2D[n.x, n.y] as AStarPathnode);
                    }
                }
            }
        }


        [ContextMenu("UpdateVisuals")]
        private void UpdateCT(){
            foreach (Tile t in tiles)
            {
                Color c = Color.white;

                if(t.index == current) c = Color.blue;
                else if (t.index == target) c = Color.red;
                else if (!t.isValid) c = Color.black;

                t.SetColor(c);
            }
        }

        private int CalcGCost(AStarPathnode a, AStarPathnode b){
            int dst = Mathf.RoundToInt(Vector2Int.Distance((a as Tile).index, (b as Tile).index) * 10);
            return dst;
        }

        private void GeneratePath(List<Tile> nodes){
            foreach(Tile n in nodes){
                n.SetColor(Color.blue);
            }
        }


        [ContextMenu("Generate Path")]
        private void GeneratePath(){
            List<Tile> path = new List<Tile>();
            StartCoroutine(PathFinding<Tile>.AStar(tiled2D[target.x, target.y], tiled2D[current.x, current.y], CalcGCost, GeneratePath, .01f));
        }
    }

}
