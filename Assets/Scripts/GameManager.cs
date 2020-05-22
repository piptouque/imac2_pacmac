
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Cinemachine;


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
        public CinemachineVirtualCamera _cam; 

        public TileGenerator _tileGen;
        public GameObject _pacman;
        public GameObject _pacdot;
        public GameObject _superpellet;
        public GameObject _powerpellet;

        // Start is called before the first frame update
        void Start()
        {
            _pacman.SetActive(false);
            _pacdot.SetActive(false);
            _superpellet.SetActive(false);
            _powerpellet.SetActive(false);
            /* */
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
            SpawnPellets(freeTiles, conf);
            CentreCamera(grid);
        }
        private void CentreCamera(TileType[,] grid)
        {
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            Vector3 centre = new Vector3(dimX / 2, dimY / 2, -1.0f);
            _cam.transform.position = centre;
            _cam.m_Lens.OrthographicSize = System.Math.Max(dimX, dimY) / 2 + 2;
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
            SpawnGameObject(_pacman, pos);
        } 


        private void SpawnPellets(List<Vector2Int> freeTiles, Configuration conf)
        {
            /*
             * a pacdot on all tiles,
             * a super whatever if roll sucessful
             */
             foreach(var tile in freeTiles)
             {
                GameObject pellet;
                switch(conf.RandomPellet())
                {
                    case Pellet.SUPER:
                        pellet =_superpellet;
                        break;
                    case Pellet.POWER:
                        pellet =_powerpellet;
                        break;
                    case Pellet.DOT: default:
                        pellet = _pacdot;
                        break;
                }
                SpawnGameObject(pellet, tile);
             }
        }
        
        private void SpawnGameObject(GameObject obj, Vector2Int pos)
        {
            // obj.SetActive(false);
            Vector3 pos3D = new Vector3((float)pos[0] + 0.5f, (float)pos[1] + 0.5f, 0);
            var objCopy = (GameObject) Object.Instantiate(obj, pos3D, Quaternion.identity);
            objCopy.SetActive(true);
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
