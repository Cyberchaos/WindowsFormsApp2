using System;
using System.IO;

namespace WindowsFormsApp2
{
	public class Map {
		private char[,] map;
		private Unit[] unitArr;
        private Building[] buildingArr;

        public Building[] BuildingArr { get => buildingArr; set => buildingArr = value; }
        public Unit[] UnitArr { get => unitArr; set => unitArr = value; }

        public Map(int numUnits, int numBuildings)
        {
            map = new char[20, 20];
            UnitArr = new Unit[numUnits];
            BuildingArr = new Building[numBuildings];
            clearMap();

            populateMap();
        }

        private void clearMap()
        {
            for (int k = 0; k < 20; k++)
            {
                for (int l = 0; l < 20; l++)
                {
                    map[k, l] = '.';
                }
            }
        }

        public void updateMap(int x, int y, char symbol)
		{
			map[x, y] = symbol;
		}

		public override string ToString()
		{
			string mapString = "";

			for (int k = 0; k < 20; k++)
			{
				for (int l = 0; l < 20; l++)
				{
					mapString += map[k, l] + " ";
				}
				mapString += "\n";
			}

			return mapString;
		}

        public void populateBuildings(Random r) {
            for (int k = 0; k < BuildingArr.Length; k++)
            {
                char team = '0';
                char symbol = 'z';
                if (r.Next(0, 2) == 0) //hero
                {
                    team = 'H';

                    if (r.Next(0, 2) == 0) //resource or fact
                    {
                        symbol = 'W';
                    }
                    else
                    {
                        symbol = 'F';
                    }
                }
                else
                {//enemy
                    team = 'E';
                    if (r.Next(0, 2) == 0) //resource or fact
                    {
                        symbol = 'w';
                    }
                    else
                    {
                        symbol = 'f';
                    }
                }
                int x = r.Next(0, 20);
                int y = r.Next(0, 20);

                if (symbol == 'w' || symbol == 'W')
                {
                    BuildingArr[k] = new ResourceBuilding(x, y, team, symbol, "Wood", 100, 2);
                }
                else if (symbol == 'f' || symbol == 'F')
                {
                    string unitType = "";
                    if (r.Next(0, 2) == 0)
                    {
                        unitType = "Melee";
                    }
                    else unitType = "Ranged";
                        BuildingArr[k] = new FactoryBuilding(x, y, team, symbol, unitType, 5);
                }
                map[x, y] = symbol;
            }
        }

        public void populateUnits(Random r) {

            for (int k = 0; k < UnitArr.Length; k++)
            {
                char team = '0';
                char symbol = 'z';
                if (r.Next(0, 2) == 0) //hero
                {
                    team = 'H';

                    if (r.Next(0, 2) == 0) //melee or ranged
                    {
                        symbol = 'M';
                    }
                    else
                    {
                        symbol = 'R';
                    }
                }
                else
                {//enemy
                    team = 'E';
                    if (r.Next(0, 2) == 0) //melee or ranged
                    {
                        symbol = 'm';
                    }
                    else
                    {
                        symbol = 'r';
                    }
                }
                int x = r.Next(0, 20);
                int y = r.Next(0, 20);

                if (symbol == 'm' || symbol == 'M')
                {
                    string name = "";
                    if (r.Next(0, 2) == 0)
                    {
                        name = "Knight";
                    }
                    else name = "Foot Soldier";
                    UnitArr[k] = new MeleeUnit(x, y, team, symbol, name);
                }
                else if (symbol == 'r' || symbol == 'R')
                {
                    string name = "";
                    if (r.Next(0, 2) == 0)
                    {
                        name = "Sniper";
                    }
                    else name = "Archer";
                    UnitArr[k] = new RangedUnit(x, y, team, symbol, name);
                }
                map[x, y] = symbol;
            }

        }

		public void populateMap()
		{

			Random r = new Random();
            populateBuildings(r);
            populateUnits(r);
           			
		}

		public void moveUnit(Unit unit, int destX, int destY) {

			Console.WriteLine("Unit " + unit.Symbol + " at [" + unit.X + "," + unit.Y + "] wants to move to [" + destX + "," + destY + "]");
			map[destX, destY] = unit.Symbol;
			map[unit.X, unit.Y] = '.';
			updateUnit(unit, destX, destY);
		}

		public void updateUnit(Unit unit, int x, int y)
		{
			unit.X = x;
			unit.Y = y;
		}

        public void loadUnits()
        {
            FileStream saveFile = new FileStream("saves/units.game", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(saveFile);
            int numUnits = File.ReadAllLines("saves/units.game").Length;
            Unit[] loadedUnits = new Unit[numUnits];
            string line = reader.ReadLine();
            int count = 0;
            Unit createdUnit = null;
            while (line != null)
            {
                string[] temp = line.Split(',');
                //determine what unit to make
                //build the unit
                //assign the unit to the array
                if ((temp[0] == "R") || (temp[0] == "r"))
                {
                    //ranged
                    createdUnit = new RangedUnit(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[3]), temp[4].ToCharArray()[0], temp[0].ToCharArray()[0], temp[1]);
                    createdUnit.Hp = Convert.ToInt32(temp[5]);
                }
                else
                {
                    //melee
                    createdUnit = new MeleeUnit(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[3]), temp[4].ToCharArray()[0], temp[0].ToCharArray()[0], temp[1]);
                    createdUnit.Hp = Convert.ToInt32(temp[5]);
                }
                loadedUnits[count] = createdUnit;
                Console.WriteLine(createdUnit.ToString());
                map[createdUnit.X, createdUnit.Y] = createdUnit.Symbol;
                line = reader.ReadLine();
                count++;
            }
            Console.WriteLine("Data read");
            reader.Close();
            saveFile.Close();
            unitArr = loadedUnits;
        }

        public void loadBuildings()
        {

            FileStream saveFile = new FileStream("saves/buildings.game", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(saveFile);
            int numBuildings = File.ReadAllLines("saves/buildings.game").Length;
            Building[] loadedBuildings = new Building[numBuildings];
            int count = 0;
            string line = reader.ReadLine();
            Building createdBuilding = null;

            while (line != null)
            {
                string[] temp = line.Split(',');
                //determine what unit to make
                //build the unit
                //assign the unit to the array
                if ((temp[0] == "W") || (temp[0] == "w"))
                {
                    //resource
                    //writer.WriteLine(Symbol + "," + Team + "," + X + "," + Y + "," + Hp + "," + ResourceType + "," + Remaining + "," + ResourcesPerTick);
                    createdBuilding = new ResourceBuilding(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[3]), temp[1].ToCharArray()[0], temp[0].ToCharArray()[0], temp[5], Convert.ToInt32(temp[6]), Convert.ToInt32(temp[7]), Convert.ToInt32(temp[8]));
                    createdBuilding.Hp = Convert.ToInt32(temp[4]);
                }
                else
                {
                    //factory
                    createdBuilding = new FactoryBuilding(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[3]), temp[1].ToCharArray()[0], temp[0].ToCharArray()[0], temp[5], Convert.ToInt32(temp[6]));
                    createdBuilding.Hp = Convert.ToInt32(temp[4]);
                  
                }
                loadedBuildings[count] = createdBuilding;
                Console.WriteLine(createdBuilding.ToString());
                map[createdBuilding.X, createdBuilding.Y] = createdBuilding.Symbol;
                line = reader.ReadLine();
                count++;
            }
            Console.WriteLine("Data read");
            reader.Close();
            saveFile.Close();
            buildingArr = loadedBuildings;
        }

        public void load() {
            clearMap();
            loadUnits();
            loadBuildings();
        }
	}




}
