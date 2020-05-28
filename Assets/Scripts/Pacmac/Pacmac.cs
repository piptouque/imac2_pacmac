
using UnityEngine;
using System;

namespace pacmac
{
    public class Pacmac : Character, ICloneable
    {

        private PacmacState _state;
        private int _score = 0;
        private int _pelletEatenCount;
        private float _powerTime; /* in seconds */ 

        public object Clone()
        {
            return MemberwiseClone();
        }


        public Pacmac()
        : base()
        {
            _state = new PacmacState();
            _pelletEatenCount = 0;
        }

        public void Set(float speedBase, float powerTime)
        {
            _speedBase = speedBase;
            _powerTime = powerTime;
        }

        override public void Update()
        {
            _state.Update(this);
        }

        override public float GetSpeed()
        {
            return _state.GetSpeed(this);
        }

        override public void Reset(Configuration conf)
        {
            Reset();
        }

        override public void Reset()
        {
            _pelletEatenCount = 0;
            _state.Reset();
        }

        public void FightGhost(Ghost ghost)
        {
            _state.FightGhost(this, ghost);
        }


        override public bool HeDed()
        {
            return _state.HeDed();
        }

        public void EatPellet(Pellet pellet)
        {
            _state.EatPellet(this, pellet); 
        }
        public int GetScore()
        {
            return _score;
        }

        public float GetPowerTime()
        {
            return _powerTime;
        }


        public void SetScore(int score)
        {
            _score = score;
        }

        public int GetPelletEatenCount()
        {
            return _pelletEatenCount;
        }

        public void ResetScore()
        {
            _score = 0;
        }

        public void SetPelletEatenCount(int pelletEatenCount)
        {
            _pelletEatenCount = pelletEatenCount;
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

        public float GetSpeed(Pacmac pacmac)
        {
            float speed = pacmac.GetSpeedBase();
            switch(_type)
            {
                case PacmacStateType.SUPER:
                    speed *= 2;
                    break;
                case PacmacStateType.NORMAL:
                case PacmacStateType.POWER:
                default:
                    break;
            }
            return speed;
        }
        private void AddScore(Pacmac pacmac, int addedBase)
        {
            int added = addedBase;
            switch(_type)
            {
                case PacmacStateType.SUPER:
                    added *= 2;
                    break;
                case PacmacStateType.NORMAL:
                case PacmacStateType.POWER:
                default:
                    break;
            }
            pacmac.SetScore(added + pacmac.GetScore());
        }

        public void EatPellet(Pacmac pacmac, Pellet pellet)
        {
            pacmac.SetPelletEatenCount(pacmac.GetPelletEatenCount() + 1);
            SetState(pellet);
            int added = pellet.GetEaten();
            AddScore(pacmac, + added);
        }

        public void FightGhost(Pacmac pacmac, Ghost ghost)
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

        public void Update(Pacmac pacmac)
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

        private void EatGhost(Pacmac pacmac, Ghost ghost)
        {
            int added = ghost.GetEaten();
            pacmac.SetScore(pacmac.GetScore() + added);
        }

        private void GetPowerOwned(Pacmac pacmac, Ghost ghost)
        {
            /* he ded */
            SetType(PacmacStateType.DED);
        }

        private void SetState(Pellet pellet)
        {
            switch(pellet.GetPalletType())
            {
                case PelletType.SUPER:
                    SetType(PacmacStateType.SUPER);
                    break;
                case PelletType.POWER:
                    SetType(PacmacStateType.POWER);
                    break;
                case PelletType.DOT:
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