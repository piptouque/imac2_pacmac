using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace pacmac
{
    public class PelletBehaviour : MonoBehaviour
    {

        public int _score = 10;
        public Pellet _type;
        public int GetEaten()
        {
            Destroy(this.gameObject);
            return _score;
        }

        public Pellet GetPelletType()
        {
            return _type;
        }

        public void Reset()
        {
            Destroy(this.gameObject);
        }
    }

}
