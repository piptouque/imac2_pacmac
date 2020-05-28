
using UnityEngine;
using UnityEngine.SceneManagement;

namespace pacmac
{
    public class GameManager : MonoBehaviour
    {
        public GameObject _levelManager;
        public GameObject _gameOverManager;
        private Configuration _conf;
        void Start()
        {
            RestartGame();
        }

        public void RestartGame()
        {
            _levelManager.SetActive(false);
            _gameOverManager.SetActive(false);

            _conf = new Configuration();
            GoToLevel(new Pacmac());
        }
        public void GoToLevel(Pacmac player)
        {
            // SceneManager.LoadScene(1, LoadSceneMode.Additive);
            // SceneManager.UnloadSceneAsync(2);
            var sceneLevel = SceneManager.GetSceneByBuildIndex(1);
            SceneManager.SetActiveScene(sceneLevel);

            _levelManager.SetActive(true);
            _levelManager.GetComponent<LevelManager>().StartLevel(_conf, player);
        }

        private void Unload()
        {
            
        }
        public void GoToGameOver(Pacmac player)
        {
            _levelManager.SetActive(false);
            // SceneManager.UnloadSceneAsync(1);
            // SceneManager.LoadScene(2, LoadSceneMode.Additive);
            var sceneGameOver = SceneManager.GetSceneByBuildIndex(2);
            SceneManager.SetActiveScene(sceneGameOver);

            _gameOverManager.SetActive(true);
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