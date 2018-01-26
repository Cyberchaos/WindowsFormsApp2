using System;
using System.IO;

namespace WindowsFormsApp2
{
	public class RangedUnit : Unit {
		public RangedUnit(int x, int y, char team, char symbol, string name) : base(x, y, 8, 2, 2, 2, team, symbol, name) {
		}

		~RangedUnit()
		{
            Console.WriteLine(this.Name + " " + this.Symbol + " at [" + this.X + "," + this.Y + "] has died.");
        }

		public override Unit closestUnit(Unit[] unitArr)
		{

			int shortestRange = 40;
			Unit returnedUnit = null;
			foreach (Unit enemy in unitArr)
			{
				if (enemy != null && this != enemy && enemy.Team != this.Team && !enemy.isDead())
				{
					int distance = Math.Abs(this.X - enemy.X) + Math.Abs(this.Y - enemy.Y);
					if (distance < shortestRange)
					{
						returnedUnit = enemy;
						shortestRange = distance;
					}

				}
			}

			return returnedUnit;
		}
		public override void combat(Unit enemy) {

			enemy.Hp -= this.Attack;
		}
		public override int move(Unit closest) {
			//Up = 1, left = 2, right = 3, down = 4
			
			//determine values between X and Y
			int distX = this.X - closest.X;
			int distY = this.Y - closest.Y;
			int returnVal = 0;

			//Check which is Absolutely smaller (we must move in that axis)
			//if (distX == 0 && distY == 0) {//they're on the same space, just don't move
			//returnVal = 0;
			//}
			//else 
			if ((Math.Abs(distX) <= Math.Abs(distY)) && (distX != 0))
			{
				//move in X

				if (distX < 0)
				{
					returnVal = 3;
				}
				else if (distX > 0)
				{
					returnVal = 2;
				}
			}
			else if (distX == 0)
			{
				//move in Y, because X is 0
				if (distY < 0)
				{
					returnVal = 4;
				}
				else if (distY > 0) {
					returnVal = 1;
				}
			}
			else if (distY == 0)
			{
				//move in X, because Y is 0

				if (distX < 0)
				{
					returnVal = 3;
				}
				else if (distX > 0)
				{
					returnVal = 2;
				}
			}
			else if ((Math.Abs(distX) > Math.Abs(distY)) && (distY != 0))
			{
				//move in Y
				if (distY < 0)
				{
					returnVal = 4;
				}
				else if (distY > 0)
				{
					returnVal = 1;
				}


			}
			return returnVal;
		}

		public override bool canAttack(Unit closest)
		{
			int distX = Math.Abs(this.X - closest.X);
			int distY = Math.Abs(this.Y - closest.Y);
			int distMax = Math.Max(distX, distY);

			bool returnVal = false;

			Console.WriteLine("I am " + this.Symbol + " at [" + this.X + "," + this.Y + "] " +" My HP: " + this.Hp + " My Range: " + this.Range + " my maximumDistance is " + distMax + "my closest enemy is " + closest.Symbol + " at [" + closest.X + "," + closest.Y + "] their HP: " + closest.Hp);

			if (this.Range >= distMax)
			{

				Console.WriteLine("returning true");
				returnVal = true;

			}
			return returnVal;
		}

		public override int run()
		{
			Random r = new Random();
			int returnVal = 0;
			bool valid = false;
			while (valid == false)
			{
				returnVal = r.Next(1, 5);
				switch (returnVal)
				{
					case 1:
						if (this.Y - 1 >= 0)
							valid = true;
						break;
					case 2:
						if (this.X - 1 >= 0)
							valid = true;
						break;
					case 3:
						if (this.X + 1 < 20)
							valid = true;
						break;
					case 4:
						if (this.Y + 1 < 20)
							valid = true;
						break;
				}
			}

			return returnVal;
		}

		public override bool isDead()
		{

			if (Hp <= 0)
				return true;
			else return false;
		}

		public override string ToString() {

			return "I am a " + this.Name +" working for Team " + this.Team + " so I show up as " + this.Symbol + "\nI am positioned at [" + this.X + "," + this.Y + "] " + "\nMy HP: " + this.Hp + "\nMy Range: " + this.Range + "\nMy Attack: " + this.Attack + "\nMy Speed: " + this.Speed;
		}

        public override void save()
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
                Console.WriteLine("Created the directory");
            }
            if (!File.Exists("saves/units.game"))
            {
                File.Create("saves/units.game").Close();
                Console.WriteLine("Created the file");
            }
            FileStream saveFile = new FileStream("saves/units.game", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(saveFile);
            //int x, int y, char team, char symbol, string name, int hp
            writer.WriteLine(Symbol + "," + Name + "," + X + "," + Y + "," + Team + "," + Hp);
            Console.WriteLine("Data written");
            writer.Close();
            saveFile.Close();
        }

    }




}
