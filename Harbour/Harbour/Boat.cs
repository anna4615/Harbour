using System;

namespace Harbour
{
    class Boat
    {
        public string IdNumber { get; set; }
        public int Weight { get; set; }
        public int MaximumSpeed { get; set; }
        public int DaysInHarbour { get; set; }


        public Boat(int weight, int maxSpeed, int daysInHarbour)
        {
            Weight = weight;
            MaximumSpeed = maxSpeed;
            DaysInHarbour = daysInHarbour;
        }

        static Random r = new Random();

        public virtual string GenerateID()
        {
            string id = "";

            for (int i = 0; i < 3; i++)
            {
                int number = r.Next(26);
                char c = (char)('A' + number);
                id += c;
            }

            return id;
        }

        public virtual string TextToFile()
        {
            return $"{IdNumber};{Weight};{MaximumSpeed}";
        }
    }
}
