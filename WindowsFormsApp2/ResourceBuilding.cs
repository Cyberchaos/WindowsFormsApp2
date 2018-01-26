using System;
using System.IO;

namespace WindowsFormsApp2
{
    public class ResourceBuilding : Building
    {
        private string resourceType;
        private int resourcesPerTick;
        private int remaining;
        private int resources = 0;

        public string ResourceType { get => resourceType; set => resourceType = value; }
        public int ResourcesPerTick { get => resourcesPerTick; set => resourcesPerTick = value; }
        public int Remaining { get => remaining; set => remaining = value; }
        public int Resources { get => resources; set => resources = value; }

        public ResourceBuilding(int x, int y, char team, char symbol, string resource, int start, int perTick) : base(x, y,50, team, symbol) {
            ResourceType = resource;
            Remaining = start;
            ResourcesPerTick = perTick;
        }
    
                public ResourceBuilding(int x, int y, char team, char symbol, string resource, int start, int perTick, int resourcesGenerated) : base(x, y, 50, team, symbol)
        {
            ResourceType = resource;
            Remaining = start;
            ResourcesPerTick = perTick;
            resources = resourcesGenerated;
        }

        public bool Generate() {
            bool returnVal = false;
            if (remaining >= resourcesPerTick)
            {
                returnVal = true;
                remaining -= resourcesPerTick;
                resources += resourcesPerTick;
            }
            else returnVal = false;
            return returnVal;
        }
        ~ResourceBuilding() {
            Console.WriteLine("Resource Building " + this.Symbol + " at [" + this.X + "," + this.Y + "] has died.");
        }

        public override string ToString() {
            return "I am a Resource Building working for Team " + this.Team + " so I show up as " + this.Symbol + "\nI am positioned at [" + this.X + "," + this.Y + "] " + "\nMy HP: " + this.Hp + "\nMy Resource: " + this.resourceType + "\nMy Resources Per Tick: " + this.resourcesPerTick + "\nI have generated " + resources + " " + this.resourceType +  "\nMy Remaining Resources: " + this.Remaining;
        }
        public override void save()
        {
            FileStream saveFile = new FileStream("saves/buildings.game", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(saveFile);
            //int x, int y, char team, char symbol, string resource, int start, int perTick
            writer.WriteLine(Symbol + "," + Team + "," + X + "," + Y + "," + Hp + "," + ResourceType + "," + Remaining + "," + ResourcesPerTick + "," + Resources);
            Console.WriteLine("Data written");
            writer.Close();
            saveFile.Close();
        }

    }
}
