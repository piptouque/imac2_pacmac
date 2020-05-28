

namespace pacmac
{
    public enum PelletType : int
    {
        DOT,
        SUPER,
        POWER
    }

    public static class PelletExtension
    {
        public static PelletType MIN_PELLETS = PelletType.DOT;
        public static PelletType MAX_PELLETS = PelletType.POWER;
        public static PelletType[] PelletList() => new PelletType[] { PelletType.DOT, PelletType.SUPER, PelletType.POWER };
    }

    public class Pellet
    {
        private bool _isEaten;
        private int _score = 10;
        private PelletType _type;

        public Pellet(PelletType type, int score)
        {
            _type = type;
            _score = score;
            _isEaten = false;
        }

        public PelletType GetPalletType()
        {
            return _type;
        }

        public bool IsEaten()
        {
            return _isEaten;
        }

        public int GetEaten()
        {
            _isEaten = true;
            return _score;
        }
    }

}