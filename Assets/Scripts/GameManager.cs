
using UnityEngine;
using UnityEngine.SceneManagement;

namespace pacmac
{
    public class GameManager : MonoBehaviour
    {
        public GameObject _levelManager;
        public GameObject _gameOverManager;
        public GameObject _menuManager;

        public GameObject _levelWrapper;
        public GameObject _gameOverWrapper;
        public GameObject _menuWrapper;
        private Configuration _conf;


        private bool _shouldRestart;
        void Start()
        {
            ClearActive();
            _conf = new Configuration();

            SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);

            _shouldRestart = true;
        }

        private void Update()
        {
            /*
             * fixit: Scenes are not loaded when building standalone
             * 
             */ 
            var sceneLevel = SceneManager.GetSceneByName("Level");
            if(_shouldRestart && sceneLevel.isLoaded)
            {
                _shouldRestart = false;
                RestartGame();
            }
        }


        public void RestartGame()
        {
            ClearActive();
            _conf.Reset();
            GoToLevel(new Pacmac());
        }

        private void ClearActive()
        {
            _levelManager.SetActive(false);
            _gameOverManager.SetActive(false);
            _menuManager.SetActive(false);

            _levelWrapper.SetActive(false);
            _gameOverWrapper.SetActive(false);
            _menuWrapper.SetActive(false);
        }
        public void GoToLevel(Pacmac player)
        {
            ClearActive();
            // SceneManager.LoadScene(1, LoadSceneMode.Additive);
            // SceneManager.UnloadSceneAsync(2);
            var sceneLevel = SceneManager.GetSceneByName("Level");
            SceneManager.SetActiveScene(sceneLevel);

            SceneManager.MoveGameObjectToScene(_levelWrapper, sceneLevel);
            _levelManager.SetActive(true);
            _levelWrapper.SetActive(true);
            _levelManager.GetComponent<LevelManager>().StartLevel(_conf, player);
        }
        
        public void GoToLevel(Configuration conf, Pacmac player)
        {
            _conf = conf;
           GoToLevel(player); 
        }

        public void GoToMenu(Pacmac player)
        {
            ClearActive();
            var sceneMenu = SceneManager.GetSceneByName("Menu");
            SceneManager.SetActiveScene(sceneMenu);

            SceneManager.MoveGameObjectToScene(_menuWrapper, sceneMenu);
            _menuManager.SetActive(true);
            _menuWrapper.SetActive(true);
            _menuManager.GetComponent<MenuManager>().PausePlay(_conf, player);
        }

        public void GoToGameOver(Pacmac player)
        {
            ClearActive();
            // SceneManager.UnloadSceneAsync(1);
            // SceneManager.LoadScene(2, LoadSceneMode.Additive);
            var sceneGameOver = SceneManager.GetSceneByName("GameOver");
            SceneManager.SetActiveScene(sceneGameOver);

            SceneManager.MoveGameObjectToScene(_gameOverWrapper, sceneGameOver);
            _gameOverManager.SetActive(true);
            _gameOverWrapper.SetActive(true);
            _gameOverManager.GetComponent<GameOverManager>().EndPlay(_conf, player);
        }

        public Configuration GetConf()
        {
            return _conf;
        }


        public static GameObject SpawnGameObject(GameObject obj, Vector2Int pos, GameObject parent)
        {
            Vector3 pos3D = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f);
            var objCopy = (GameObject) Object.Instantiate(
                obj,
                pos3D,
                Quaternion.identity,
                parent.transform
                );
            return objCopy;
        }
    }
}