using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class RowingBoat : Boat
    {
        public int MaximumPassengers { get; set; }

        public RowingBoat(int weight, int maxSpeed, int daysInHarbour, int maxPassengers) : base(weight, maxSpeed, daysInHarbour)
        {
            IdNumber = "R-" + GenerateID();
            MaximumPassengers = maxPassengers;
        }

        public override string ToString()
        {
            return $"Roddbåt\t\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tMax antal pers: {MaximumPassengers}\tStannar {DaysInHarbour} dagar";
        }
    }
}
