
namespace pacmac
{
    public class Ghost : Character
    {
            private int _score = 100;
            private float _speed;

            public Ghost(int score, float speedBase)
            : base(speedBase)
            {
                _score = score;
            }

            override public float GetSpeed()
            {
                return _speed;
            }

            override public void Reset(Configuration conf)
            {
                _speed = GetSpeedBase() * conf.RandomGhostSpeed();
                _isDead = false;
            }

            override public void Reset()
            {
                _isDead = false;
            }


            public int GetEaten()
            {
                _isDead = true;
                return GetScore();
            }
            public int GetScore()
            {
                return _score;
            }
    }

}