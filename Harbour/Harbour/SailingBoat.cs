using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class SailingBoat : Boat
    {
        public string Type { get; set; }
        public int Length { get; set; }

        public SailingBoat(int weight, int maxSpeed, int daysStaying, int daysSinceArrival, int length)
            : base(weight, maxSpeed, daysStaying, daysSinceArrival)
        {
            Type = "Segelbåt";
            IdNumber = "S-" + GenerateID();
            Length = length;
        }

        public override string ToString()
        {
            return $"{Type}\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tLängd: {Length}       \tStannar {DaysSinceArrival} dagar";
        }
        public override string TextToFile(int index)
        {
            return base.TextToFile(index) + $"{Type};{DaysStaying};{DaysSinceArrival};{Length}";
        }
    }
}
