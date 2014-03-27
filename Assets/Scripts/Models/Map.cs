using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Map : MonoBehaviour
    {
        public Tile[,] Tiles;
        public List<Transform> Walls;
        public List<Transform> WallCaps;
        public List<Transform> Doors; 

        public List<Texture> Materials;

        public GameObject TileModel;
        public GameObject WallModel;
        public GameObject WallCapModel;
        public GameObject DoorModel;

        public float TileXOffset = 1; //TODO: Update the model later so you don't do this in code!

        public string LevelName;

        void Start()
        {
            LoadMaterials();
            TileModel = Resources.Load<GameObject>("Tiles/Tile");
            WallModel = Resources.Load<GameObject>("Models/Wall");
            WallCapModel = Resources.Load<GameObject>("Models/WallCap");
            DoorModel = Resources.Load<GameObject>("Models/Door");

            LoadLevel();
        }

        private void LoadMaterials()
        {
            Materials = Resources.LoadAll<Texture>("Tiles").ToList();
        }

        #region Level Loading

        public void LoadLevel()
        {
            var doc = new XmlDocument();
            doc.Load("Assets/Resources/Levels/" + LevelName + ".xml");

            var level = doc.SelectSingleNode("/Level");
            var rows = level.SelectNodes("Row");
            var c = rows[0].SelectNodes("Column");

            Tiles = new Tile[c.Count, rows.Count];

            CreateFloor(rows);
            CreateWalls(level);
            CreateWallCaps();
            CreateDoors(level);
        }

        private void CreateFloor(XmlNodeList rows)
        {
            for (int y = 0; y < rows.Count; y++)
            {
                var columns = rows[y].SelectNodes("Column");
                for (int x = 0; x < columns.Count; x++)
                {
                    var currentNode = rows[y].SelectNodes("Column")[x];
                    var t = new Tile { Transform = (Instantiate(TileModel) as GameObject).transform };
                    t.Transform.position = new Vector3((x + TileXOffset), 0, y);
                    t.Transform.renderer.material.mainTexture =
                        Materials[int.Parse(currentNode.SelectSingleNode("Tile").InnerText)];
                    t.Transform.gameObject.SetActive(true);
                    Tiles[x, y] = t;
                }
            }
        }

        private void CreateWalls(XmlNode level)
        {
            Walls = new List<Transform>();
            var walls = level.SelectSingleNode("Walls");
            foreach (XmlNode wall in walls.SelectNodes("Wall"))
            {
                var pos = wall.InnerText.Split(' ');
                var start = new Vector3(float.Parse(pos[0].Split(',')[0]), 0, float.Parse(pos[0].Split(',')[1]));
                var end = new Vector3(float.Parse(pos[1].Split(',')[0]), 0, float.Parse(pos[1].Split(',')[1]));
                var newPos = start;

                var needed = Vector3.Distance(start, end);
                var dir = start.x / end.x == 1;

                for (int i = 0; i < needed; i++)
                {
                    var w = (Instantiate(WallModel) as GameObject).transform;
                    w.position = newPos;
                    w.Rotate(0, dir ? 0 : 90, 0);
                    w.gameObject.SetActive(true);
                    Walls.Add(w);

                    newPos = Vector3.MoveTowards(newPos, end, 1);
                }
            }
        }

        private void CreateWallCaps()
        {
            WallCaps = new List<Transform>();
            foreach (var wall in Walls)
            {
                var check = Walls.Where(x => x.rotation == wall.rotation).ToList();
                if (Math.Abs(wall.rotation.eulerAngles.y - 90) < .1)
                {
                    if (check.All(x => x.transform.position != wall.position + Vector3.right))
                        AddCap(wall.position, Vector3.right);
                    if (check.All(x => x.transform.position != wall.position + Vector3.left))
                        AddCap(wall.position);
                }
                else
                {
                    if (check.All(x => x.transform.position != wall.position + Vector3.forward))
                        AddCap(wall.position, Vector3.forward);
                    if (check.All(x => x.transform.position != wall.position + Vector3.back))
                        AddCap(wall.position);
                }
            }
        }

        private void AddCap(Vector3 position, Vector3 offset = default(Vector3))
        {
            if (WallCaps.Any(x => x.position == position + offset)) return;

            var wc = (Instantiate(WallCapModel) as GameObject).transform;
            wc.position = position + offset;
            wc.gameObject.SetActive(true);
            WallCaps.Add(wc);
        }

        private void CreateDoors(XmlNode level)
        {
            Doors = new List<Transform>();
            var doors = level.SelectSingleNode("Doors");
            foreach (XmlNode door in doors.SelectNodes("Door"))
            {
                var configs = door.InnerText.Split(',');

                var d = (Instantiate(DoorModel) as GameObject).transform;
                d.Translate(new Vector3(float.Parse(configs[0]), .5f, float.Parse(configs[1])) +
                            (Vector3.right * TileXOffset / 2));
                d.Rotate(0, float.Parse(configs[2]), 0);

                if (Math.Abs(d.rotation.eulerAngles.y - 90) < .1)
                    d.Translate(-.5f, 0, -.5f);

                SetActive(d.gameObject, false);
                SetActive(d.gameObject, true);
                Doors.Add(d);
            }
        }

        void SetActive(GameObject go, bool activate)
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.SetActive(activate);
            }
        }

        #endregion
    }
}
