

namespace pacmac
{
    public enum Pellet : int
    {
        DOT,
        SUPER,
        POWER
    }

    public static class PelletExtension
    {
        public static Pellet MIN_PELLETS = Pellet.DOT;
        public static Pellet MAX_PELLETS = Pellet.POWER;
        public static Pellet[] PelletsList() => new Pellet[] { Pellet.DOT, Pellet.SUPER, Pellet.POWER };
    }

}