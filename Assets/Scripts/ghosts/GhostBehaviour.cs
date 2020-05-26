using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public abstract class GhostBehaviour : CharacterBehaviour 
    {
        [SerializeField]
        private int _score = 100;
        private float _speed;
        protected Vector2 _prevDir;
        protected Vector2[] _dirPriorities = new Vector2[4];

        public int GetEaten()
        {
            /*
             * for now, we RESET it
             * that's right
             * nothin' personal, though
             */
            ResetPosition();
            return _score;
        }

        override protected float GetSpeed()
        {
            return _speed;
        }

        override protected Vector2 ChooseDest()
        {
            Vector2 dest = (Vector2) transform.position;
            Vector2 dir = _prevDir;
            if (IsDirectionValid(_prevDir))
            {
                dest += _prevDir;
            }
            else
            {
            var rotToPrev = Quaternion.FromToRotation(Vector2.up, _prevDir);
                for (int i=0; i<_dirPriorities.GetLength(0); ++i)
                {
                    /*
                     * directions are in the referential of the ghost.
                     */
                    dir = rotToPrev * _dirPriorities[i];
                    if (IsDirectionValid(dir))
                    {
                        dest += dir;
                        break;
                    }
                }
            }
            _prevDir = dir;
            return dest;
        } 

        override public void ResetPosition()
        {
            base.ResetPosition();
            _prevDir = _dirPriorities[0];
        }

        override public void ResetPosition(Vector3 pos3D, Configuration conf)
        {  
            base.ResetPosition(pos3D, conf);
            _speed = GetSpeedBase() * conf.RandomGhostSpeed();
            _prevDir = _dirPriorities[0];
        }
    }

}

