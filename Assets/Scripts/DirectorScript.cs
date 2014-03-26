using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Assets.Scripts.Models;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    private Map _map;

    public Transform TileModel;
    public Transform WallModel;
    public Transform WallCapModel;
    public Transform DoorModel;

    public float TileSize = 2;
    public float TileXOffset = 1; //TODO: Update the model later so you don't do this in code!

    // Use this for initialization
    void Start()
    {
        _map = new Map();
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateMap()
    {
        CreateTiles("TestLevel");
    }

    private void CreateTiles(string name)
    {
        var doc = new XmlDocument();
        doc.Load("Assets/Resources/Levels/" + name + ".xml");

        var level = doc.SelectSingleNode("/Level");
        var rows = level.SelectNodes("Row");
        var c = rows[0].SelectNodes("Column");

        _map.Tiles = new Tile[c.Count, rows.Count];

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
                var t = new Tile { Transform = Instantiate(TileModel) as Transform };
                t.Transform.position = new Vector3((x + TileXOffset) * TileSize, 0, y * TileSize);
                t.Transform.localScale *= TileSize;
                t.Transform.renderer.material.mainTexture =
                    _map.Materials[int.Parse(currentNode.SelectSingleNode("Tile").InnerText)];
                t.Transform.gameObject.SetActive(true);
                _map.Tiles[x, y] = t;
            }
        }
    }

    private void CreateWalls(XmlNode level)
    {
        _map.Walls = new List<Transform>();
        var walls = level.SelectSingleNode("Walls");
        foreach (XmlNode wall in walls.SelectNodes("Wall"))
        {
            var pos = wall.InnerText.Split(' ');
            var start = new Vector3(float.Parse(pos[0].Split(',')[0]), 0, float.Parse(pos[0].Split(',')[1]));
            var end = new Vector3(float.Parse(pos[1].Split(',')[0]), 0, float.Parse(pos[1].Split(',')[1]));
            var newPos = start;

            var needed = Vector3.Distance(start, end);
            var dir = start.x/end.x == 1;

            for (int i = 0; i < needed; i++)
            {
                var w = Instantiate(WallModel) as Transform;
                w.position = newPos*TileSize;
                w.localScale *= TileSize;
                w.Rotate(0, dir ? 0 : 90, 0);
                w.gameObject.SetActive(true);
                _map.Walls.Add(w);

                newPos = Vector3.MoveTowards(newPos, end, 1);
            }
        }
    }

    private void CreateWallCaps()
    {
        _map.WallCaps = new List<Transform>();
        foreach (var wall in _map.Walls)
        {
            var check = _map.Walls.Where(x => x.rotation == wall.rotation).ToList();
            if (Math.Abs(wall.rotation.eulerAngles.y - 90) < .1)
            {
                if (check.All(x => x.transform.position != wall.position + (Vector3.right * TileSize)))
                    AddCap(wall.position, new Vector3(TileSize, 0, 0));
                if (check.All(x => x.transform.position != wall.position + (Vector3.left * TileSize)))
                    AddCap(wall.position);
            }
            else
            {
                if (check.All(x => x.transform.position != wall.position + (Vector3.forward * TileSize)))
                    AddCap(wall.position, new Vector3(0, 0, TileSize));
                if (check.All(x => x.transform.position != wall.position + (Vector3.back * TileSize)))
                    AddCap(wall.position);
            }
        }
    }

    private void AddCap(Vector3 position, Vector3 offset = default(Vector3))
    {
        var wc = Instantiate(WallCapModel) as Transform;
        wc.position = position + offset;
        wc.localScale *= TileSize;
        wc.gameObject.SetActive(true);
        _map.WallCaps.Add(wc);
    }

    private void CreateDoors(XmlNode level)
    {
        _map.Doors = new List<Transform>();
        var doors = level.SelectSingleNode("Doors");
        foreach (XmlNode door in doors.SelectNodes("Door"))
        {
            var configs = door.InnerText.Split(',');

            var d = Instantiate(DoorModel) as Transform;
            d.Translate((new Vector3(float.Parse(configs[0]), 1, float.Parse(configs[1])) * TileSize) + 
                        (Vector3.right * TileXOffset));
            d.localScale *= TileSize;
            d.Rotate(0, float.Parse(configs[2]), 0);
            SetActive(d.gameObject, false);
            SetActive(d.gameObject, true);
            _map.Doors.Add(d);
        }
    }

    void SetActive(GameObject go, bool activate)
    {
        foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.active = activate;
        }
    }
}