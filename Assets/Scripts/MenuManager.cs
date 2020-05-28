
using UnityEngine;
using System.Linq;

namespace pacmac
{
    public class MenuManager : MonoBehaviour
    {
        Configuration _conf;
        Pacmac _player;
        public GameObject _valuePDim;
        public GameObject _valuePPath;
        public GameObject _valueFacGhostSpeed;
        public GameObject[] _valuesPsPellet;


        public void PausePlay(Configuration conf, Pacmac player)
        {
            Load(conf, player);
        }
        
        private void SetUI(Configuration conf)
        {
            double pDim = _conf.GetPDim();
            double pPath = _conf.GetPPath();
            double facGhostSpeed = _conf.GetFacGhostSpeed();
            double[] psPellet = _conf.GetPsPellet();
            _valuePDim.GetComponent<ButtonBehaviour>().SetValue(pDim);
            _valuePPath.GetComponent<ButtonBehaviour>().SetValue(pPath);
            _valueFacGhostSpeed.GetComponent<ButtonBehaviour>().SetValue(facGhostSpeed);
            for(int i=0; i<psPellet.GetLength(0); ++i)
            {
               _valuesPsPellet[i].GetComponent<ButtonBehaviour>().SetValue(psPellet[i]);
            }
        }

        private Configuration SetConfiguration(Configuration conf)
        {
            double pDim = _valuePDim.GetComponent<ButtonBehaviour>().GetValue();
            double pPath = _valuePPath.GetComponent<ButtonBehaviour>().GetValue();
            double facGhostSpeed = _valueFacGhostSpeed.GetComponent<ButtonBehaviour>().GetValue();
            var psPellet = new double[_valuesPsPellet.GetLength(0)];
            for(int i=0; i<_valuesPsPellet.GetLength(0); ++i)
            {
                psPellet[i] = _valuesPsPellet[i].GetComponent<ButtonBehaviour>().GetValue();
            } 
            /* need to normalise  !! */
            double norm = psPellet.Sum();
            psPellet = psPellet.Select(p => p / norm).ToArray();
            conf.SetPDim(pDim);
            conf.SetPPath(pPath);
            conf.SetFacGhostSpeed(facGhostSpeed);
            conf.SetPsPellet(psPellet);

            return conf;
        }

        private void Load(Configuration conf, Pacmac player)
        {
            _conf = conf;
            _player = player;

           SetUI(conf);
        }

        private Configuration Unload(Configuration conf)
        {
            conf = SetConfiguration(conf);
            return conf;
        }


        private void UnpausePlay(Configuration conf, Pacmac player)
        {
            conf = Unload(conf);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().GoToLevel(conf, player);
        }


        void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                UnpausePlay(_conf, _player);
            }
        }
    }

}