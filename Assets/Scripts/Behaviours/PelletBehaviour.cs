using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace pacmac
{
    public class PelletBehaviour : MonoBehaviour
    {

        [SerializeField]
        private int _score = 10;
        [SerializeField]
        private PelletType _type = PelletType.DOT;
        public Pellet _pellet;

        void Start()
        {
            _pellet = new Pellet(_type, _score);
        }

        void FixedUpdate()
        {
            if(GetPellet().IsEaten())
            {
                EatenUpdate();
            }
        }
        public void EatenUpdate()
        {
            Reset();
        }

        public Pellet GetPellet()
        {
            return _pellet;
        }

        public void Reset()
        {
            Destroy(this.gameObject);
        }
    }

}
