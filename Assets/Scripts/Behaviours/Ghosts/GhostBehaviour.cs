using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public abstract class GhostBehaviour : CharacterBehaviour 
    {
        [SerializeField]
        private int _score = 40;
        private Ghost _char;
        protected Vector2 _prevDir;
        protected Vector2[] _dirPriorities = new Vector2[4];

        public GhostBehaviour()
        {
            _char = new Ghost(_score, _speedBase);
            _prevDir = Vector2.zero;
        }

        override protected Character GetCharacter()
        {
            return (Character) _char;
        }

        public Ghost GetGhost()
        {
            return _char;
        }

        override protected void DeadUpdate()
        {
            ResetPosition();
        }

        public int GetEaten()
        {
            /*
             * for now, we RESET it
             * that's right
             * nothin' personal, though
             */
            return _score;
        }

        override protected Vector2 ChooseDest()
        {
            Vector2 dest = (Vector2) transform.position;
            Vector2 dir = _prevDir == Vector2.zero ? _dirPriorities[0] : _prevDir;
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
            _char.Reset();
            _prevDir = Vector2.zero;
        }

        override public void ResetPosition(Vector3 pos3D, Configuration conf)
        {
            base.ResetPosition(pos3D, conf);
            _char.Reset(conf);
            _prevDir = Vector2.zero;
        }
    }

}

