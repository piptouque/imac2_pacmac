
using UnityEngine;

using pacmac.random;

namespace pacmac
{
    public class GameOverManager : MonoBehaviour
    {
        private Configuration _conf;
        public GameObject[] _valuesDim;
        public GameObject[] _valuesPath;
        public GameObject[] _valuesGhostSpeed;
        public GameObject[] _valuesPelletType;
        void Awake()
        {
            DeactivateBase();
        }
        
        private void DeactivateBase()
        {
        }

        private void SpawnTexts()
        {

        }

        private void Load(Configuration conf)
        {
            _conf = conf;
        }

        private void Unload()
        {
        }

        private static void SetUIResult<T>(GameObject[] values, RandomMemoryResult<T> res)
        {
            values[0].GetComponent<TMPro.TMP_Text>().text = res.MemoryMean().ToString("0.####");
            values[1].GetComponent<TMPro.TMP_Text>().text = res.MemoryStandardDeviation().ToString("0.####");
        }
        private void SetUI()
        {
            RandomMemoryResult<int> resDim = _conf.GetDimResults();
            RandomMemoryResult<int> resPath = _conf.GetNumberPathsResults();
            RandomMemoryResult<double> resGhostSpeed = _conf.GetGhostSpeedResults();
            RandomMemoryResult<PelletType> resPelletType = _conf.GetPelletTypesResults();

            {
                SetUIResult(_valuesDim, resDim);
                SetUIResult(_valuesPath, resPath);
                SetUIResult(_valuesGhostSpeed, resGhostSpeed);
                SetUIResult(_valuesPelletType, resPelletType);
            }
        }

        public void EndPlay(Configuration conf, Pacmac player)
        {
            Load(conf);
            SetUI();
        }

        private void RestartPlay()
        {
            Unload();
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RestartGame();
        }


        void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                RestartPlay();
            }
        }
    }

}