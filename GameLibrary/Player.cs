namespace GameLibrary
{
    public class Player
    {
        private int _exp;
        public int Exp
        {
            get => _exp;
            set
            {
                if (LevelUp(ref value, Lvl))
                {
                    Lvl++;
                }
                _exp = value;
            }
        }

        public Player(int id)
        {
            Id = id;
            Lvl = 1;
            Items = new Inventory();
        }

        public int Id { get; set; }
        public int Lvl { get; set; }
        public Inventory Items { get; set; }
        public double Money { get; set; }
        public int Donate { get; set; }

        private static bool LevelUp(ref int exp, int lvl)
        {
            if (exp >  LvlCap[lvl])
            {
                exp -= LvlCap[lvl];
                return true;
            }

            return false;
        }
        private static readonly int[] LvlCap = { 0, 10, 20, 50, 80, 150, 300, 500, 750, 1000 };
    }
}