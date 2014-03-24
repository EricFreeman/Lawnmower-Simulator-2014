using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    private Map _map;

	// Use this for initialization
	void Start ()
	{
	    _map = CreateMap();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private Map CreateMap()
    {
        var m = new Map();

        var width = 10;
        var height = 10;

        m.Tiles = new Tile[width,height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var t = new Tile {GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube)};
                t.GameObject.transform.position = new Vector3(x, 0, y);
                t.GameObject.renderer.material = new Material("Diffuse");
                m.Tiles[x, y] = t;
            }

        return m;
    }
}
