
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Cinemachine;


namespace pacmac
{
    public class LevelManager : MonoBehaviour
    {
        private int _level = 0;
        private Configuration _conf;
        private TileGenerator _tileGen;
        private GameObject _wrapper;
        private GameObject _pacmac;
        private GameObject _blinky;
        private GameObject _inky;
        private GameObject _pinky;
        private GameObject _clyde;
        private GameObject[] _pellets;

        public Tilemap _wallMap;
        public Tilemap _rimMap;
        public RuleTile _wallTiles;
        public RuleTile _rimTiles;
        public CinemachineVirtualCamera _cam; 

        /* Characters base  */
        public GameObject _pacmacBase;
        public GameObject _blinkyBase;
        public GameObject _inkyBase;
        public GameObject _pinkyBase;
        public GameObject _clydeBase;
        /* Pellets base */
        public GameObject _pacdotBase;
        public GameObject _superpelletBase;
        public GameObject _powerpelletBase;
        /* UI */
        public GameObject  _scoreCount;
        public GameObject  _levelCount;


        void Awake()
        {
            DeactivateBase();
        }

        private void DeactivateBase()
        {
            _pacmacBase.SetActive(false);
            _blinkyBase.SetActive(false);
            _inkyBase.SetActive(false);
            _pinkyBase.SetActive(false);
            _clydeBase.SetActive(false);
            _pacdotBase.SetActive(false);
            _superpelletBase.SetActive(false);
            _powerpelletBase.SetActive(false);
        }

        private void Load(Configuration conf)
        {
            /* */
            _tileGen = new TileGenerator();
            _conf = conf;

            /* */
                    
            /* */
            _wrapper =  new GameObject("WrapperLevel");
            SpawnCharacters();
        }

        private void Load(Configuration conf, Pacmac player)
        {
            Load(conf);
            _pacmac.GetComponent<PacmacBehaviour>().SetPacmac(player);
        }

        private Pacmac Unload()
        {
            var player = (Pacmac) _pacmac.GetComponent<PacmacBehaviour>().GetPacmac().Clone();
            Object.Destroy(_wrapper);
            return player;
        }

        public void StartLevel(Configuration conf, Pacmac player)
        {
            Load(conf, player);
            ResetLevel(0);
        }

        void FixedUpdate()
        {
            if (IsLevelFinished())
            {
                ResetLevel(_level + 1);
            }
            else if (IsPacmacDead())
            {
                GoToGameOver();
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                GoToMenu();
            }
            else
            {
                DisplayUI();
            }
        }
        private void GoToGameOver()
        {
            var manager = GameObject.FindWithTag("GameManager");
            Pacmac player = Unload();
            manager.GetComponent<GameManager>().GoToGameOver(player);
        }

        private void GoToMenu()
        {
            var manager = GameObject.FindWithTag("GameManager");
            Pacmac player = Unload();
            manager.GetComponent<GameManager>().GoToMenu(player);
        }

        private bool IsLevelFinished()
        {
            return _pellets.GetLength(0) == _pacmac.GetComponent<PacmacBehaviour>().GetPelletEatenCount();
        }

        private bool IsPacmacDead()
        {
            return _pacmac.GetComponent<PacmacBehaviour>().HeDed();
        }

        private void DisplayUI()
        {
            int score = _pacmac.GetComponent<PacmacBehaviour>().GetScore();
            _scoreCount.GetComponent<TMPro.TMP_Text>().text = System.Convert.ToString(score);
            _levelCount.GetComponent<TMPro.TMP_Text>().text = System.Convert.ToString(_level);
        }

        private void ResetLevel(int level)
        {
            _level = level;
            _conf.Reset(_level);

            TileType[,] grid = _tileGen.GenerateTiles(_conf);
            int dimX = grid.GetLength(0);
            int dimY = grid.GetLength(1);
            Vector2Int dim = new Vector2Int(dimX, dimY);
            List<Vector2Int> freeTiles = FindFreeTiles(grid);

            ResetGrid(grid);
            ResetPositionCharacters(freeTiles, _conf);
            ResetPellets(freeTiles, _conf);
            CentreCamera(dim);
        }
        private void CentreCamera(Vector2Int dim)
        {
            int dimX = dim[0];
            int dimY = dim[1];
            Vector3 centre = new Vector3(dimX / 2, (dimY + 1)/ 2 , -1.0f);
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

        private void ResetScorePlayer()
        {
            _pacmac.GetComponent<PacmacBehaviour>().ResetScore();
        }

        private void ResetPositionCharacters(List<Vector2Int> freeTiles, Configuration conf)
        {
            ResetPositionCharacter<PacmacBehaviour>(_pacmac, freeTiles, conf);
            ResetPositionCharacter<BlinkyBehaviour>(_blinky, freeTiles, conf);
            ResetPositionCharacter<InkyBehaviour>  (_inky,   freeTiles, conf);
            ResetPositionCharacter<PinkyBehaviour> (_pinky,  freeTiles, conf);
            ResetPositionCharacter<ClydeBehaviour> (_clyde,  freeTiles, conf);
        }

        private void ResetPositionCharacter<T>(GameObject character, List<Vector2Int> freeTiles, Configuration conf)
            where T : CharacterBehaviour 
        {
            /* find a random empty tile ? */
            /* nope, first one, whatever. */
            if(freeTiles.Count == 0)
            {
                throw new System.ArgumentException("Nope, not happening.");
            }
            int randomIndex = conf.RandomUniformInt(0, freeTiles.Count - 1);
            Vector2Int pos = freeTiles[randomIndex];
            var pos3D = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f);
            character.GetComponent<T>().ResetPosition(pos3D, conf);
            freeTiles.RemoveAt(randomIndex);
        } 


        private void SpawnCharacters()
        {
            _pacmac = SpawnCharacter(_pacmacBase);
            _blinky = SpawnCharacter(_blinkyBase);
            _inky   = SpawnCharacter(_inkyBase);
            _pinky  = SpawnCharacter(_pinkyBase);
            _clyde  = SpawnCharacter(_clydeBase);
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
                switch(conf.RandomPelletType())
                {
                    case PelletType.SUPER:
                        pellet = _superpelletBase;
                        break;
                    case PelletType.POWER:
                        pellet = _powerpelletBase;
                        break;
                    case PelletType.DOT: default:
                        pellet = _pacdotBase;
                        break;
                }
                _pellets[i++] = SpawnPellet(pellet, tile);
            }
        }
        
        private GameObject SpawnPellet(GameObject pellet, Vector2Int pos)
        {
            var pelletCopy = GameManager.SpawnGameObject(pellet, pos, _wrapper);
            pelletCopy.SetActive(true);
            return pelletCopy;
        }

        private GameObject SpawnCharacter(GameObject character)
        {
            return GameManager.SpawnGameObject(character, Vector2Int.zero, _wrapper);
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
