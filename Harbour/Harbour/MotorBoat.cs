using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class MotorBoat : Boat
    {
        public int Power { get; set; }

        public MotorBoat(int weight, int maxSpeed, int daysInHarbour, int power) : base(weight, maxSpeed, daysInHarbour)
        {
            IdNumber = "M-" + GenerateID();
            Power = power;
        }
        public override string ToString()
        {
            return $"Motorbåt\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tHästkrafter: {Power}   \tStannar {DaysInHarbour} dagar";
        }
        public override string TextToFile()
        {
            return $"Motorbåt;" + base.TextToFile();
        }
    }


}
