using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class CargoShip : Boat
    {
        public int Containers { get; set; }

        public CargoShip(int weight, int maxSpeed, int daysInHarbour, int containers) : base(weight, maxSpeed, daysInHarbour)
        {
            IdNumber = "L-" + GenerateID();
            Containers = containers;
        }
        public override string ToString()
        {
            return $"Lastfartyg\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tContainers: {Containers}   \tStannar {DaysInHarbour} dagar";
        }
        public override string TextToFile()
        {
            return $"Lastfartyg;" + base.TextToFile();
        }
    }
}
    