﻿
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
        private Configuration _conf;
        private GameObject _pacman;
        private GameObject[] _pellets;

        public Tilemap _wallMap;
        public Tilemap _rimMap;
        public RuleTile _wallTiles;
        public RuleTile _rimTiles;
        public CinemachineVirtualCamera _cam; 

        public TileGenerator _tileGen;
        public GameObject _pacmanBase;
        public GameObject _pacdotBase;
        public GameObject _superpelletBase;
        public GameObject _powerpelletBase;
        /* UI */
        public GameObject  _scoreCount;
        public GameObject  _levelCount;

        // Start is called before the first frame update
        void Start()
        {
            _pacmanBase.SetActive(false);
            _pacdotBase.SetActive(false);
            _superpelletBase.SetActive(false);
            _powerpelletBase.SetActive(false);
            /* */
            _tileGen = new TileGenerator();
            _conf = new Configuration();

            /* */
            SpawnPlayer();

            /* */
            ResetLevel(_level, _conf);
        }

        void FixedUpdate()
        {
            DisplayScore(); 
            DisplayLevel();
        }

        void Update()
        {
            if(IsLevelFinished())
            {
                ResetLevel(++_level, _conf);
            }
        }

        private bool IsLevelFinished()
        {
            return _pellets.GetLength(0)== _pacman.GetComponent<PacmacBehaviour>().GetPalletEatenCount();
        }

        private void DisplayScore()
        {
            int score = _pacman.GetComponent<PacmacBehaviour>().GetScore();
            _scoreCount.GetComponent<TMPro.TMP_Text>().text = System.Convert.ToString(score);
        }
        private void DisplayLevel()
        {
            _levelCount.GetComponent<TMPro.TMP_Text>().text = System.Convert.ToString(_level);
        }

        private void ResetLevel(int level, Configuration conf)
        {
            conf.Reset(level);

            TileType[,] grid = _tileGen.GenerateTiles(conf);
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            Vector2Int dim = new Vector2Int(dimX, dimY);
            List<Vector2Int> freeTiles = FindFreeTiles(grid);

            ResetGrid(grid);
            ResetPlayer(freeTiles, conf);
            ResetPellets(freeTiles, conf);
            CentreCamera(dim);
        }
        private void CentreCamera(Vector2Int dim)
        {
            int dimX = dim[0];
            int dimY = dim[1];
            Vector3 centre = new Vector3(dimX / 2, dimY / 2, -1.0f);
            _cam.transform.position = centre;
            _cam.m_Lens.OrthographicSize = System.Math.Max(dimX, dimY) / 2 + 4;
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

        private void ResetPlayer(List<Vector2Int> freeTiles, Configuration conf)
        {
            /* find a random empty tile ? */
            /* nope, first one, whatever. */
            if(freeTiles.Count == 0)
            {
                throw new System.ArgumentException("Nope, not happening.");
            }
            Vector2Int pos = freeTiles[0];
            var pos3D = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f);
            _pacman.GetComponent<PacmacBehaviour>().Reset(pos3D);
        } 

        private void SpawnPlayer()
        {
            _pacman = SpawnGameObject(_pacmanBase, Vector2Int.zero);
        }


        private void ResetPellets(List<Vector2Int> freeTiles, Configuration conf)
        {
            /*
             * a pacdot on all tiles,
             * a super whatever if roll sucessful
             */
            if(_pellets != null)
            {
                foreach(var pellet in _pellets)
                {
                    if(pellet != null)
                    {
                        pellet.GetComponent<PelletBehaviour>().Reset();
                    }
                }
            }
            int i = 0;
            _pellets = new GameObject[freeTiles.Count];
            foreach(var tile in freeTiles)
            {
                GameObject pellet;
                switch(conf.RandomPellet())
                {
                    case Pellet.SUPER:
                        pellet = _superpelletBase;
                        break;
                    case Pellet.POWER:
                        pellet = _powerpelletBase;
                        break;
                    case Pellet.DOT: default:
                        pellet = _pacdotBase;
                        break;
                }
                _pellets[i++] = SpawnGameObject(pellet, tile);
            }
        }
        
        private GameObject SpawnGameObject(GameObject obj, Vector2Int pos)
        {
            // obj.SetActive(false);
            Vector3 pos3D = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
            var objCopy = (GameObject) Object.Instantiate(obj, pos3D, Quaternion.identity);
            objCopy.SetActive(true);
            return objCopy;
        }

        private void ResetGrid(TileType[,] grid)
        {
            /* */
            _wallMap.ClearAllTiles();
            _rimMap.ClearAllTiles();
            /* */
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