using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class MotorBoat : Boat
    {
        public string Type { get; set; }
        public int Power { get; set; }

        public MotorBoat(int weight, int maxSpeed, int daysStaying, int daysSinceArrival, int power)
            : base(weight, maxSpeed, daysStaying, daysSinceArrival)
        {
            Type = "Motorbåt";
            IdNumber = GenerateID();
            Power = power;
        }
        public override string GenerateID()
        {
            return "M-" + base.GenerateID();
        }

        public override string ToString()
        {
            return $"{Type}\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tHästkrafter: {Power}   \tStannar {DaysSinceArrival} dagar";
        }
        public override string TextToFile(int index)
        {
            return base.TextToFile(index) + $"{Type};{DaysStaying};{DaysSinceArrival};{Power}";
        }
    }


}