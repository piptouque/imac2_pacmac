
using UnityEngine;

namespace pacmac
{
    public class GameOverManager : MonoBehaviour
    {
        public GameObject _gameOverUIBase;

        private GameObject _gameOverUI;
        public GameObject _wrapper;

        void Awake()
        {
            DeactivateBase();
        }
        
        private void DeactivateBase()
        {
            _gameOverUIBase.SetActive(false);
        }

        private void SpawnTexts()
        {
            _gameOverUI = GameManager.SpawnGameObject(_gameOverUIBase, Vector2Int.zero, _wrapper);
            _gameOverUI.SetActive(true);
        }

        private void Load()
        {
           _wrapper = new GameObject("Wrapper");

           SpawnTexts();
        }

        private void Unload()
        {
            Object.Destroy(_wrapper);
        }

        public void EndPlay(Configuration conf, Pacmac player)
        {
            Load();
        }

        private void RestartPlay()
        {
            Unload();
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RestartGame();
        }


        void FixedUpdate()
        {
            if(Input.GetKey(KeyCode.Return))
            {
                RestartPlay();
            }
        }
    }

}