using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class SailingBoat : Boat
    {
        public int Length { get; set; }

        public SailingBoat(int weight, int maxSpeed, int daysInHarbour, int length) : base(weight, maxSpeed, daysInHarbour)
        {
            IdNumber = "S-" + GenerateID();
            Length = length;
        }

        public override string ToString()
        {
            return $"Segelbåt\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tHästkrafter: {Length}\tStannar {DaysInHarbour} dagar";
        }
    }
}
