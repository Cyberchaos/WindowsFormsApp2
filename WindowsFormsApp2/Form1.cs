using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;


namespace WindowsFormsApp2
{

    public partial class Form1 : Form
	{
		GameEngine engine = new GameEngine(6, 4);

		System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();

		public Form1()
		{
			InitializeComponent();

            cmbUnits.DataSource = RefreshBox();
        }

		private void label1_Click(object sender, EventArgs e)
		{

		}

		public void updateMap(object o, EventArgs e) {
			lblMap.Text = engine.Map.ToString();
			lblTime.Text = engine.Ticks.ToString();
            int index = cmbUnits.SelectedIndex;
            cmbUnits.DataSource = RefreshBox();
            cmbUnits.SelectedIndex = index;
        }
		
			private void button1_Click(object sender, EventArgs e)
		{
			time.Tick += new EventHandler(updateMap);
			time.Interval = 1000;
			time.Enabled = true;
			lblMap.Text = engine.Map.ToString();
			lblTime.Text = "0";
			engine.Time.Enabled = true;
		}

		private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
		{
            //cmbUnits.DataSource = RefreshBox();
            if (cmbUnits.SelectedIndex < engine.Map.UnitArr.Length)
            {
                Console.WriteLine(cmbUnits.SelectedIndex);
               if (engine.Map.UnitArr[cmbUnits.SelectedIndex] != null)
                {
                    lblUnit.Text = engine.Map.UnitArr[cmbUnits.SelectedIndex].ToString();
                }
                else
                    lblUnit.Text = "This unit has died. Please select another unit.";
            }
            else {
                var buildingIndex = cmbUnits.SelectedIndex - engine.Map.UnitArr.Length;
               if (engine.Map.BuildingArr[buildingIndex] != null)
                {
                    lblUnit.Text = engine.Map.BuildingArr[buildingIndex].ToString();
                }
                else
                    lblUnit.Text = "This building has died. Please select another building.";
            }
		}

        public string[] RefreshBox()
        {
            Unit[] unitArr = engine.Map.UnitArr;
            Building[] buildingArr = engine.Map.BuildingArr;
            string[] returnString = new string[unitArr.Length + buildingArr.Length];

                for (int k = 0; k < unitArr.Length; k++)
                {
                    if (unitArr[k] != null)
                    {
                        if (unitArr[k].Symbol == 'M')
                            returnString[k] =("Hero Team: Melee Unit");
                        if (unitArr[k].Symbol == 'R')
                            returnString[k] =("Hero Team: Ranged Unit");
                        if (unitArr[k].Symbol == 'm')
                            returnString[k] =("Enemy Team: Melee Unit");
                        if (unitArr[k].Symbol == 'r')
                            returnString[k] =("Enemy Team: Ranged Unit");
                    }
                    else
                    {
                        returnString[k] =("Dead Unit");
                    }
                }

                    for (int k = 0; k < buildingArr.Length; k++)
                    {
                        if (buildingArr[k] != null)
                        {
                            if (buildingArr[k].Symbol == 'W')
                                returnString[unitArr.Length + k] =("Hero Team: Resource Building");
                            if (buildingArr[k].Symbol == 'F')
                                returnString[unitArr.Length + k] =("Hero Team: Unit Factory");
                            if (buildingArr[k].Symbol == 'w')
                                returnString[unitArr.Length + k] =("Enemy Team: Resource Building");
                            if (buildingArr[k].Symbol == 'f')
                                returnString[unitArr.Length + k] =("Enemy Team: Unit Factory");
                        }
                        else
                        {
                            returnString[unitArr.Length + k] =("Dead Building");
                        }
               
            }
            return returnString;
        }

		private void btnPause_Click(object sender, EventArgs e)
		{
			time.Enabled = false;
			engine.Time.Enabled = false;
		}

        private void btnSave_Click(object sender, EventArgs e)
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
            if (!File.Exists("saves/buildings.game"))
            {
                File.Create("saves/buildings.game").Close();
                Console.WriteLine("Created the file");
            }
            File.Delete("saves/units.game");
            File.Delete("saves/buildings.game");
            for (int k = 0; k < engine.Map.UnitArr.Length; k++)
            {
                if (engine.Map.UnitArr[k] != null) {
                    engine.Map.UnitArr[k].save();
                }
            }

            for (int k = 0; k < engine.Map.BuildingArr.Length; k++)
            {
                if (engine.Map.BuildingArr[k] != null)
                {
                    engine.Map.BuildingArr[k].save();
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            if (Directory.Exists("saves") && (File.Exists("saves/units.game")) && (File.Exists("saves/buildings.game")))
            {
                time.Enabled = false;
                engine.Time.Enabled = false;
                engine.Ticks = 0;
                engine.Map.load();
                lblMap.Text = engine.Map.ToString();
                int index = cmbUnits.SelectedIndex;
                cmbUnits.DataSource = RefreshBox();
                cmbUnits.SelectedIndex = index;
            }
            else lblUnit.Text = "Nothing to Load";

        }
    }
}
