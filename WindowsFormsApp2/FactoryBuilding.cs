using System;
using System.IO;

namespace WindowsFormsApp2
{
    public class FactoryBuilding : Building {
        private string unitType;
        private int speed;
        private int spawnX;
        private int spawnY;

        public string UnitType { get => unitType; set => unitType = value; }
        public int Speed { get => speed; set => speed = value; }
        public int SpawnX { get => spawnX; set => spawnX = value; }
        public int SpawnY { get => spawnY; set => spawnY = value; }

        public FactoryBuilding(int x, int y, char team, char symbol, string units, int speed) : base(x, y,25, team, symbol) {
            UnitType = units;
            Speed = speed;
            SpawnX = x;
            if (y != 0)
            {
                spawnY = y - 1;
            }
            else {
                spawnY = y + 1;
            }
        }

        public Unit generateUnit() {
            Unit unit = null;

            Random r = new Random();
            string name = "";
            if (UnitType == "Melee") {
            if (r.Next(0, 2) == 0)
            {
                name = "Knight";
            }
            else name = "Foot Soldier";
        }
            if (UnitType == "Ranged")
            {
                if (r.Next(0, 2) == 0)
                {
                    name = "Sniper";
                }
                else name = "Archer";
            }

            if (UnitType == "Melee" && (Team == 'H'))
            {
                unit = new MeleeUnit(SpawnX, SpawnY, Team, 'M', name);
            }
            else if (UnitType == "Melee" && (Team == 'E')) {
                unit = new MeleeUnit(SpawnX, SpawnY, Team, 'm', name);
            }
            else if (UnitType == "Ranged" && (Team == 'H'))
            {
                unit = new RangedUnit(SpawnX, SpawnY, Team, 'R', name);
            }
            else if (UnitType == "Ranged" && (Team == 'E'))
            {
                unit = new RangedUnit(SpawnX, SpawnY, Team, 'r', name);
            }

            return unit;
        }

        ~FactoryBuilding()
        {
            Console.WriteLine("Factory Building " + this.Symbol + " at [" + this.X + "," + this.Y + "] has died.");
        }

        public override string ToString()
        {
            return "I am a Factory Building working for Team " + this.Team + " so I show up as " + this.Symbol + "\nI am positioned at [" + this.X + "," + this.Y + "] " + "\nMy HP: " + this.Hp + "\nI create: " + this.unitType + " units every " + this.speed + " seconds at [" + this.spawnX + ", " + this.spawnY + "]";
        }

        public override void save()
        {
            FileStream saveFile = new FileStream("saves/buildings.game", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(saveFile);
            //int x, int y, char team, char symbol, string resource, int start, int perTick
            writer.WriteLine(Symbol + "," + Team + "," + X + "," + Y + "," + Hp + "," + unitType + "," + speed);
            Console.WriteLine("Data written");
            writer.Close();
            saveFile.Close();
        }
    }
}
