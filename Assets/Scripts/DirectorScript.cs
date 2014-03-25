using System.Xml;
using Assets.Scripts.Models;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    private Map _map;

    public Transform TileModel;
    public float TileSize = 2;

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

        for (int y = 0; y < rows.Count; y++)
        {
            var columns = rows[y].SelectNodes("Column");
            for (int x = 0; x < columns.Count; x++)
            {
                var currentNode = rows[y].SelectNodes("Column")[x];
                var t = new Tile { Transform = Instantiate(TileModel) as Transform };
                t.Transform.position = new Vector3(x * TileSize, 0, y * TileSize);
                t.Transform.localScale *= TileSize;
                t.Transform.renderer.material.mainTexture = _map.Materials[int.Parse(currentNode.SelectSingleNode("Tile").InnerText)];
                t.Transform.gameObject.SetActive(true);
                _map.Tiles[x, y] = t;
            }
        }
    }
}
