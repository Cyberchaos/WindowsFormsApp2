using System;

namespace WindowsFormsApp2
{
	public abstract class Unit {
		private int x;
		private int y;
		private int hp;
		private int speed;
		private int attack;
		private int range;
		private char team;
		private char symbol;
		private bool combatFlag = false;
		private int maxHP;
        private string name;

		public int X
		{
			get
			{
				return x;
			}

			set
			{
				x = value;
			}
		}

		public int Y
		{
			get
			{
				return y;
			}

			set
			{
				y = value;
			}
		}

		public int Hp
		{
			get
			{
				return hp;
			}

			set
			{
				hp = value;
			}
		}

		public int Speed
		{
			get
			{
				return speed;
			}

			set
			{
				speed = value;
			}
		}

		public int Attack
		{
			get
			{
				return attack;
			}

			set
			{
				attack = value;
			}
		}

		public int Range
		{
			get
			{
				return range;
			}

			set
			{
				range = value;
			}
		}

		public char Team
		{
			get
			{
				return team;
			}

			set
			{
				team = value;
			}
		}

		public char Symbol
		{
			get
			{
				return symbol;
			}

			set
			{
				symbol = value;
			}
		}

		public bool CombatFlag
		{
			get
			{
				return combatFlag;
			}

			set
			{
				combatFlag = value;
			}
		}

		public int MaxHP { get { return maxHP; } set { maxHP = value; } }

        public string Name { get => name; set => name = value; }

        public Unit(int x, int y, int hp, int speed, int attack, int range, char team, char symbol, string name) {
			X = x;
			Y = y;
			Hp = hp;
			MaxHP = hp;
			Speed = speed;
			Attack = attack;
			Range = range;
			Team = team;
			Symbol = symbol;
            Name = name;
		}

		~Unit() {
		}

		public abstract Unit closestUnit(Unit[] unitArr);
		public abstract void combat(Unit enemy);
		public abstract int move(Unit closest);
		public abstract bool canAttack(Unit closest);
		public abstract int run();
		public abstract bool isDead();
		public abstract override string ToString();
        public abstract void save();

	}




}
