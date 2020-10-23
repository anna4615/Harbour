using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class CargoShip : Boat
    {
        public string Type { get; set; }
        public int Containers { get; set; }

        public CargoShip(int weight, int maxSpeed, int daysStaying, int daysSinceArrival, int containers)
            : base(weight, maxSpeed, daysStaying, daysSinceArrival)
        {
            Type = "Lastfartyg";
            IdNumber = "L-" + GenerateID();
            Containers = containers;
        }
        public override string ToString()
        {
            return $"{Type}\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tContainers: {Containers}   \tStannar {DaysSinceArrival} dagar";
        }
        public override string TextToFile(int index)
        {
            return base.TextToFile(index) + $"{Type};{DaysStaying};{DaysSinceArrival};{Containers}";
        }
    }
}
    