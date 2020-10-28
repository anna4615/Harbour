﻿using System.Collections.Generic;
using System.Linq;

namespace Harbour
{
    class RowingBoat : Boat
    {
        public int MaximumPassengers { get; set; }

        public RowingBoat(/*string type, */string id, int weight, int maxSpeed, int daysStaying, int daysSinceArrival, int maxPassengers)
            : base(weight, maxSpeed, daysStaying, daysSinceArrival)
        {
            Type = "Roddbåt";
            IdNumber = id;
            MaximumPassengers = maxPassengers;
        }



        public override string ToString()
        {
            return $"{Type}\t\t{IdNumber}\t{Weight}\t{MaximumSpeed}\tMax antal pers: {MaximumPassengers}" +
                $"\tDagar i hamn: {DaysSinceArrival}\tStannar {DaysStaying} dagar";
        }

        public override string TextToFile(int index)
        {
            return base.TextToFile(index) + $"{Type};{DaysStaying};{DaysSinceArrival};{MaximumPassengers}";
        }

        public static void CreateRowingBoat(List<Boat> boats)
        {
            string id = "R-" + GenerateID();
            int weight = Utils.r.Next(100, 300 + 1);
            int maxSpeed = Utils.r.Next(3 + 1);
            int daysStaying = 1;
            int maxPassengers = Utils.r.Next(1, 6 + 1);
            int daysSinceArrival = 0;

            RowingBoat rowingBoat = new RowingBoat(id, weight, maxSpeed, daysStaying, daysSinceArrival, maxPassengers);
            boats.Add(rowingBoat);
        }

        public static (int, bool) FindRowingboatSpace(HarbourSpace[] harbour)
        {
            int selectedSpace = 0;
            bool spaceFound = false;

            //Hitta en plats med en rodd båt redan
            foreach (var space in harbour)
            {
                foreach (var boat in space.ParkedBoats)
                {
                    if (boat is RowingBoat && space.ParkedBoats.Count() == 1)
                    {
                        selectedSpace = space.SpaceId;
                        spaceFound = true;
                        break;
                    }
                }
                if (spaceFound)
                {
                    break;
                }
            }

            // Om index 0 är ledigt och index 1 upptaget
            if (spaceFound == false)
            {
                if (harbour[0].ParkedBoats.Count == 0 && harbour[1].ParkedBoats.Count > 0)
                {
                    selectedSpace = 0;
                    spaceFound = true;
                }
            }

            if (spaceFound == false)
            {
                // Hitta ensam plats med upptagna platser runtom
                var q1 = harbour
                    .FirstOrDefault(h => h.ParkedBoats.Count == 0
                    && h.SpaceId > 0
                    && h.SpaceId < harbour.Length - 2
                    && harbour[h.SpaceId - 1].ParkedBoats.Count > 0
                    && harbour[h.SpaceId + 1].ParkedBoats.Count > 0);
                
                if (q1 != null)
                {
                    selectedSpace = q1.SpaceId;
                    spaceFound = true;
                }
            }

            if (spaceFound == false)
            {
                // Hitta första lediga plats
                var q2 = harbour
                   .FirstOrDefault(h => h.ParkedBoats.Count == 0);

                if (q2 != null)
                {
                    selectedSpace = q2.SpaceId;
                    spaceFound = true;
                }
            }

            return (selectedSpace, spaceFound);
        }
    }
}
