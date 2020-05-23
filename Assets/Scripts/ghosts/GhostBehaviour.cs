using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public abstract class GhostBehaviour : CharacterBehaviour 
    {
        public int _score = 100;
        private float _speed = 0.4f;

        public int GetEaten()
        {
            /*
             * for now, we DESTROY it
             * that's right
             * nothin' personal, though
             */
            Destroy(this.gameObject);
            return _score;
        }

        override protected float GetSpeed()
        {
            return _speed;
        }

        override protected void OnTriggerEnter2D(Collider2D other)
        {

        }

        override public void Reset(Vector3 pos3D, Configuration conf)
        {  
            base.Reset(pos3D, conf);
            _speed = conf.RandomGhostSpeed();
        }
    }

}

