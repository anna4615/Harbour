using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class RowingBoat : Boat
    {
        public string Type { get; set; }
        public int MaximumPassengers { get; set; }

        public RowingBoat(int weight, int maxSpeed, int daysStaying, int daysSinceArrival, int maxPassengers)
            : base(weight, maxSpeed, daysStaying, daysSinceArrival)
        {
            IdNumber = GenerateID();
            Type = "Roddbåt";
            MaximumPassengers = maxPassengers;
        }

        public override string GenerateID()
        {
            return "R-" + base.GenerateID();
        }

        public override string ToString()
        {
            return $"{Type}\t\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tMax antal pers: {MaximumPassengers}\tStannar {DaysSinceArrival} dagar";
        }

        public override string TextToFile(int index)
        {
            return  base.TextToFile(index) + $"{Type};{DaysStaying};{DaysSinceArrival};{MaximumPassengers}";
        }
    }
}
