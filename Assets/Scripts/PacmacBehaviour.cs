using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{

    public class PacmacBehaviour : CharacterBehaviour 
    {
        private PacmacState _state = new PacmacState();
        private int _score = 0;
        private int _pelletEatenCount = 0;
        public float _powerTime = 20.0f; /* in seconds */ 

        void Update()
        {
            _state.Update(this);
        }

        public bool HeDed()
        {
            return _state.HeDed();
        }

        public int GetScore()
        {
            return _score;
        }

        override protected float GetSpeed()
        {
            return _state.GetSpeed(this);
        }

        public float GetPowerTime()
        {
            return _powerTime;
        }


        public void SetScore(int score)
        {
            _score = score;
        }

        public void SetPelletEatenCount(int pelletEatenCount)
        {
            _pelletEatenCount = pelletEatenCount;
        }

        public int GetPelletEatenCount()
        {
            return _pelletEatenCount;
        }

        override public void Reset(Vector3 pos3D, Configuration conf)
        {
            base.Reset(pos3D, conf);
            _pelletEatenCount = 0;
            _state.Reset();
        }

        override protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pellet"))
            {
                EatPellet(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Ghost"))
            {
                FightGhost(other.gameObject);
            }
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

        private void EatPellet(GameObject pellet)
        {
            _state.EatPellet(this, pellet);
        }

        private void FightGhost(GameObject ghost)
        {
            _state.FightGhost(this, ghost);
        }
    }

    enum PacmacStateType { NORMAL, SUPER, POWER, DED }
    class PacmacState
    {

        private float _stateStartTime;

        private PacmacStateType _type;

        public PacmacState()
        {
            Reset();
        }

        public float GetSpeed(PacmacBehaviour pacmac)
        {
            float speed;
            switch(_type)
            {
                case PacmacStateType.SUPER:
                    speed = pacmac.GetSpeedBase() * 2;
                    break;
                case PacmacStateType.NORMAL:
                case PacmacStateType.POWER:
                default:
                    speed = pacmac.GetSpeedBase();
                    break;
            }
            return speed;
        }
        private void AddScore(PacmacBehaviour pacmac, int addedBase)
        {
            int added;
            switch(_type)
            {
                case PacmacStateType.SUPER:
                    added = addedBase * 2;
                    break;
                case PacmacStateType.NORMAL:
                case PacmacStateType.POWER:
                default:
                    added = addedBase;
                    break;
            }
            pacmac.SetScore(added + pacmac.GetScore());
        }

        public void EatPellet(PacmacBehaviour pacmac, GameObject pellet)
        {
            pacmac.SetPelletEatenCount(pacmac.GetPelletEatenCount() + 1);
            SetState(pellet);
            int added = pellet.GetComponent<PelletBehaviour>().GetEaten();
            AddScore(pacmac, + added);
        }

        public void FightGhost(PacmacBehaviour pacmac, GameObject ghost)
        {
            switch (_type)
            {
                case PacmacStateType.POWER: 
                    EatGhost(pacmac, ghost);
                    break;
                case PacmacStateType.NORMAL:
                case PacmacStateType.SUPER:
                default:
                    GetPowerOwned(pacmac, ghost);
                    break;
            }
        }


        public bool HeDed()
        {
            return _type == PacmacStateType.DED;
        }

        public void Update(PacmacBehaviour pacmac)
        {
            if (_type != PacmacStateType.NORMAL)
            {
                float elapsed = Time.time - _stateStartTime;
                if (elapsed > pacmac.GetPowerTime())
                {
                    Reset();
                }
            }
        }
        public void Reset()
        {
            _type = PacmacStateType.NORMAL;
        }

        private void EatGhost(PacmacBehaviour pacmac, GameObject pellet)
        {
            int added = pellet.GetComponent<GhostBehaviour>().GetEaten();
            pacmac.SetScore(pacmac.GetScore() + added);
        }

        private void GetPowerOwned(PacmacBehaviour pacmac, GameObject ghost)
        {
            /* he ded */
            SetType(PacmacStateType.DED);
        }

        private void SetState(GameObject pellet)
        {
            Pellet pelletType = pellet.GetComponent<PelletBehaviour>().GetPelletType();
            switch(pelletType)
            {
                case Pellet.SUPER:
                    SetType(PacmacStateType.SUPER);
                    break;
                case Pellet.POWER:
                    SetType(PacmacStateType.POWER);
                    break;
                case Pellet.DOT:
                default:
                    break;
            }
        }

        private void SetType(PacmacStateType type)
        {
            _type = type;
            _stateStartTime = Time.time;
        }
    }
}