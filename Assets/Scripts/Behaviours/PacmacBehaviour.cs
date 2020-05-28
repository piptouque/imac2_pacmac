using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{

    public class PacmacBehaviour : CharacterBehaviour 
    {
        [SerializeField]
        private float _powerTime = 20.0f; /* in seconds */ 
        private Pacmac _char;
        private static int _COUNT = 0;
        private int _count;


        void Start()
        {
            _count = _COUNT++;
        }

        public void SetPacmac(Pacmac character)
        {
            _char = character;
            _char.Set(_speedBase, _powerTime);
        }

        override protected void FixedUpdate()
        {
            base.FixedUpdate();
            _char.Update();
        }

        override protected void DeadUpdate()
        {
            
        }

        override protected Character GetCharacter()
        {
            return (Character) _char;
        }

        public Pacmac GetPacmac()
        {
            return _char;
        }

        override public void ResetPosition(Vector3 pos3D, Configuration conf)
        {
            base.ResetPosition(pos3D, conf);
            _char.Reset(conf);
        }

        override public void ResetPosition()
        {
            base.ResetPosition();
            _char.Reset();
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pellet"))
            {
                _char.EatPellet(other.gameObject.GetComponent<PelletBehaviour>().GetPellet());
            }
            else if (other.gameObject.CompareTag("Ghost"))
            {
                _char.FightGhost(other.gameObject.GetComponent<GhostBehaviour>().GetGhost());
            }
        }

        public int GetPelletEatenCount()
        {
            return _char.GetPelletEatenCount();
        }

        public int GetScore()
        {
            return _char.GetScore();
        }

        public void ResetScore()
        {
            _char.ResetScore();
        }


        override protected Vector2 ChooseDest()
        {
            Vector2 dest = (Vector2) transform.position;
            if(Input.GetKey(KeyCode.UpArrow) && IsDirectionValid(Vector2.up))
            {
                dest += Vector2.up;
            }
            else if(Input.GetKey(KeyCode.RightArrow) && IsDirectionValid(Vector2.right))
            {
                dest += Vector2.right;
            }
            else if(Input.GetKey(KeyCode.DownArrow) && IsDirectionValid(- Vector2.up))
            {
                dest -= Vector2.up;
            }
            else if(Input.GetKey(KeyCode.LeftArrow) && IsDirectionValid(- Vector2.right))
            {
                dest -= Vector2.right;
            }
            return dest;
        }
    }
}