using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Map
    {
        public Tile[,] Tiles;
        public List<Transform> Walls;
        public List<Transform> WallCaps;
        public List<Transform> Doors; 

        public List<Texture> Materials;

        public Map()
        {
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            Materials = Resources.LoadAll<Texture>("Tiles").ToList();
        }
    }
}
