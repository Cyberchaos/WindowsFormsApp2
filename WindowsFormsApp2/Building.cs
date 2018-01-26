namespace WindowsFormsApp2
{
    public abstract class Building {
        private int x;
        private int y;
        private int hp;
        private char team;
        private char symbol;

        public Building(int x, int y, int hp, char team, char symbol) {
            this.X = x;
            this.Y = y;
            this.Hp = hp;
            this.Team = team;
            this.Symbol = symbol;
        }

        ~Building() {
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Hp { get => hp; set => hp = value; }
        public char Team { get => team; set => team = value; }
        public char Symbol { get => symbol; set => symbol = value; }

        public abstract override string ToString();
        public abstract void save();

    }
}
