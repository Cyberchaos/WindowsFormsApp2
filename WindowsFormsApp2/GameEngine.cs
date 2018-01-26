using System;
using System.Timers;

namespace WindowsFormsApp2
{
	public class GameEngine {
		private Map map;
		private System.Timers.Timer time = new System.Timers.Timer();
		private int ticks = 0;

		public Map Map
		{
			get
			{
				return map;
			}

			set
			{
				map = value;
			}
		}

		public System.Timers.Timer Time
		{
			get
			{
				return time;
			}

			set
			{
				time = value;
			}
		}

		public int Ticks
		{
			get
			{
				return ticks;
			}

			set
			{
				ticks = value;
			}
		}

		public GameEngine(int units, int buildings) {
			Map = new Map(units, buildings);
			Time.Elapsed += new ElapsedEventHandler(playGame);
			Time.Interval = 1000;
			Time.Enabled = false;
		}

		public void moveUnit(Unit temp, int switchValue)
		{
			switch (switchValue)
			{
				case 1:
					Console.WriteLine("returned 1 from switchval");
					Map.moveUnit(temp, temp.X, temp.Y - 1);
					Console.WriteLine("Unit " + temp.Symbol + "is at [" + temp.X + "," + temp.Y + "]");
					break;
				case 2:
					Console.WriteLine("returned 2 from switchval");
					Map.moveUnit(temp, temp.X - 1, temp.Y);
					Console.WriteLine("Unit " + temp.Symbol + "is at [" + temp.X + "," + temp.Y + "]");
					break;
				case 3:
					Console.WriteLine("returned 3 from switchval");
					Map.moveUnit(temp, temp.X + 1, temp.Y);
					Console.WriteLine("Unit " + temp.Symbol + "is at [" + temp.X + "," + temp.Y + "]");
					break;
				case 4:
					Console.WriteLine("returned 4 from switchval");
					Map.moveUnit(temp, temp.X, temp.Y + 1);
					Console.WriteLine("Unit " + temp.Symbol + "is at [" + temp.X + "," + temp.Y + "]");
					break;
			}
		}


		public void playGame(object o, ElapsedEventArgs e) {

			Ticks += 1;

			for (int k = 0; k < Map.UnitArr.Length; k++) {//garbage collection loop - set to null if dead
				if (Map.UnitArr[k] != null && Map.UnitArr[k].isDead())
				{
					Map.updateMap(Map.UnitArr[k].X, Map.UnitArr[k].Y, '.');
					Map.UnitArr[k] = null;
				}
			}

            foreach (Building temp in Map.BuildingArr)
            {
                Map.updateMap(temp.X, temp.Y, temp.Symbol);
                Console.WriteLine(temp.GetType().ToString());
                if (temp.GetType().ToString() == "WindowsFormsApp2.ResourceBuilding")
                {
                    ResourceBuilding rb = (ResourceBuilding)temp;
                    if (!rb.Generate())
                        Console.WriteLine("I am " + rb.Symbol + " at [" + rb.X + "," + rb.Y + "]" + ": I am out of resources hurr durrr");
                    else Console.WriteLine(rb.ToString());
                }
                if (temp.GetType().ToString() == "WindowsFormsApp2.FactoryBuilding") {
                    FactoryBuilding fb = (FactoryBuilding)temp;
                    if ((Ticks % fb.Speed) == 0)
                    {
                        Unit newUnit = fb.generateUnit();
                        Console.WriteLine("At " + Ticks + " I was born:\n" + newUnit.ToString());

                        Unit[] newUnitArr = new Unit[Map.UnitArr.Length + 1];
                        for (int k = 0; k < Map.UnitArr.Length; k++)
                        {
                            newUnitArr[k] = Map.UnitArr[k];
                        }
                        newUnitArr[newUnitArr.Length - 1] = newUnit;
                        Map.UnitArr = newUnitArr;
                    }
                }
            }

			foreach (Unit temp in Map.UnitArr){
				//Do some stuff every second
				//I need to check if temp != null and set units to null somehow
				if (temp != null && !temp.isDead())//else he ded now
				{
					Map.updateMap(temp.X, temp.Y, temp.Symbol);
					if (temp.Hp < (0.25 * temp.MaxHP)) //else he healty, attack, continue or move
					{
						temp.CombatFlag = false;
						int switchValue = temp.run();
						Console.WriteLine("Unit " + temp.Symbol + " at [" + temp.X + "," + temp.Y + "] is running the fuck awayyyyyyyyyy!");
						moveUnit(temp, switchValue);
					}
					else //he healty, attack, continue or move
					{
						Unit nearestEnemy = temp.closestUnit(Map.UnitArr);
						if (nearestEnemy != null) //welp, we gud fam
						{
							if (temp.CombatFlag && temp.canAttack(nearestEnemy) && (nearestEnemy.isDead() != true)) //continue attacking if already
							{
								temp.combat(nearestEnemy);
								Console.WriteLine("Unit " + temp.Symbol + " at [" + temp.X + "," + temp.Y + "] would like to continue attacking its nearest enemy " + nearestEnemy.Symbol + " at [" + nearestEnemy.X + "," + nearestEnemy.Y + "]");
								Console.WriteLine("My HP = " + temp.Hp + " Enemy HP = " + nearestEnemy.Hp);
							}
							else //can't continue, maybe start?
							{
								if (!temp.CombatFlag && temp.canAttack(nearestEnemy) && (nearestEnemy.isDead() != true)) //start attacking
								{
									temp.combat(nearestEnemy);
									temp.CombatFlag = true;
									Console.WriteLine("Unit " + temp.Symbol + " at [" + temp.X + "," + temp.Y + "] would like to continue attacking its nearest enemy " + nearestEnemy.Symbol + " at [" + nearestEnemy.X + "," + nearestEnemy.Y + "]");
									Console.WriteLine("My HP = " + temp.Hp + " Enemy HP = " + nearestEnemy.Hp);
								}
								else //can't attack, just move
								{
									temp.CombatFlag = false;
									if ((Ticks % temp.Speed) == 0 && (nearestEnemy.isDead() != true)) //Can move as per game clock
									{
										int switchValue = temp.move(nearestEnemy);//returns 1, 2, 3, 4 based on direction, updateMap and view
										moveUnit(temp, switchValue);
									}
								}
							}
						}//nearestenemy is null, fuck
					}
				}

			}
		}
	}




}
