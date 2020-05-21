
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


namespace pacmac
{
    public class GameManager : MonoBehaviour
    {
        private int _level = 0;
        private float _tileSize = 1;

        public Tilemap _wallMap;
        public Tilemap _rimMap;
        public RuleTile _wallTiles;
        public RuleTile _rimTiles;

        public TileGenerator _tileGen;
        public GameObject _pacman;

        // Start is called before the first frame update
        void Start()
        {
            _tileGen = new TileGenerator();
            StartLevel(_level, new Configuration());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void StartLevel(int level, Configuration conf)
        {
            conf.reset(level);
            TileType[,] grid = _tileGen.GenerateTiles(conf);
            List<Vector2Int> freeTiles = FindFreeTiles(grid);
            FillGrid(grid);
            SpawnPlayer(freeTiles, conf);
        }

        private List<Vector2Int> FindFreeTiles(TileType[,] grid)
        {
            List<Vector2Int> freeTiles = new List<Vector2Int>();
            for(int x=0; x<grid.GetLength(0); ++x)
            {
                for(int y=0; y<grid.GetLength(1); ++y)
                {
                    if(grid[x,y] == TileType.EMPTY)
                    {
                        var freeTile = new Vector2Int(x,y);
                        freeTiles.Add(freeTile);
                    }
                }
            }
            return freeTiles;
        }

        private void SpawnPlayer(List<Vector2Int> freeTiles, Configuration conf)
        {
            /* find a random empty tile ? */
            /* nope, first one, whatever. */
            if(freeTiles.Count == 0)
            {
                throw new System.ArgumentException("Nope, not happening.");
            }
            Vector2Int pos = freeTiles[0];
            Vector3 pos3D = new Vector3((float)pos[0] + 0.5f, (float)pos[1] + 0.5f, 0);
            var pacmanCopy = (GameObject) Object.Instantiate(_pacman, pos3D, Quaternion.identity);
            pacmanCopy.SetActive(true);
            _pacman.SetActive(false);
        } 
        private void FillGrid(TileType[,] grid)
        {
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            for (int x=0; x<dimX; ++x)
            {
                _rimMap.SetTile(new Vector3Int(x, -1, 0), _rimTiles);
                _rimMap.SetTile(new Vector3Int(x, dimY, 0), _rimTiles);
                for (int y=0; y<dimY; ++y)
                {
                    switch(grid[x,y])
                    {
                        case TileType.WALL:
                            _wallMap.SetTile(new Vector3Int(x, y, 0), _wallTiles);
                            break;
                        case TileType.EMPTY: default:
                            break;
                    }    
                }
            }
            for (int y=-1; y<=dimY; ++y)
            {
                _rimMap.SetTile(new Vector3Int(-1, y, 0), _rimTiles);
                _rimMap.SetTile(new Vector3Int(dimX, y, 0), _rimTiles);
            }
        }
    }
    
}
