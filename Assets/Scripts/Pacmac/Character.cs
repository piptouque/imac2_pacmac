

namespace pacmac
{
    public abstract class Character
    {
        protected float _speedBase;
        protected bool _isDead;

        public Character(float speedBase)
        {
            _speedBase = speedBase;
            _isDead = false;
        }

        public Character()
        {
            _isDead = false;
        }

        virtual public void Update()
        {

        }

        abstract public void Reset(Configuration conf);
        abstract public void Reset();

        abstract public float GetSpeed();

        virtual public bool HeDed()
        {
            return _isDead;
        }

        public float GetSpeedBase()
        {
            return _speedBase;
        }
    }

}