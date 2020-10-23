using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Harbour
{
    class Program
    {
        //static Random r = new Random();

        static void Main(string[] args)
        {
            string fileText = File.ReadAllText("BoatsInHarbour.txt");

            Console.WriteLine(fileText);

            HarbourSpace[] harbour = new HarbourSpace[5];

            for (int i = 0; i < harbour.Length; i++)
            {
                harbour[i] = new HarbourSpace(i);
            }

            List<Boat> arrivingBoats = new List<Boat>();
            int newBoats = 2;
            StreamWriter sw = new StreamWriter("BoatsInHarbour.txt", false);

            AddNewBoats(arrivingBoats, newBoats);

            // Skapar roddbåt för test, ta bort sedan
            arrivingBoats.Add(new RowingBoat(10, 2, 1, 0, 4));

            foreach (var boat in arrivingBoats)
            {
                Console.WriteLine(boat.ToString());
            }



            foreach (var boat in arrivingBoats)
            {
                int harbourPosition = 0;

                if (boat is RowingBoat)
                {
                    harbourPosition = FindRowingboatSpace(harbour);
                    AddBoatToHarbour(boat, harbour, harbourPosition);
                }

            }





            sw.Close();
            fileText = File.ReadAllText("BoatsInHarbour.txt");
            Console.WriteLine(fileText);
            SaveToFile(sw, harbour);
            sw.Close();
        }

        private static int FindRowingboatSpace(HarbourSpace[] harbour)
        {
            //int index = 0;
            int selectedSpace = 0;
            bool spaceFound = false;
            //var q = harbour
            //    .Where(h => h.ParkedBoats.Count() == 1);


            foreach (var space in harbour)
            {
                foreach (var boat in space.ParkedBoats)
                {
                    if (boat is RowingBoat && space.ParkedBoats.Count() == 1)  // Hitta plats med en roddbåt 
                    {
                        selectedSpace = space.SpaceId;
                        spaceFound = true;
                    }
                }
                if (spaceFound)
                {
                    break;
                }
            }

            if (spaceFound == false)
            {
                var q1 = harbour
                    .Where(h => h.ParkedBoats.Count == 0);

                foreach (var space in q1)
                {
                    if (space.SpaceId == 0 && harbour[space.SpaceId + 1].ParkedBoats.Count == 1)
                    {
                        selectedSpace = space.SpaceId;
                        spaceFound = true;
                        break;
                    }
                }

                if (spaceFound == false)
                {
                    foreach (var space in q1)
                    {
                        if (space.SpaceId > 0 && space.SpaceId < harbour.Length - 2
                        && harbour[space.SpaceId - 1].ParkedBoats.Count == 1 && harbour[space.SpaceId + 1].ParkedBoats.Count == 1)  // Hitta en plats där bara roddbåt får plats
                        {
                            selectedSpace = space.SpaceId;
                            spaceFound = true;
                            break;
                        }
                    }
                }

                if (spaceFound == false)
                {
                    foreach (var space in q1)
                    {
                        if (space.SpaceId == harbour.Length - 1 && harbour[space.SpaceId - 1].ParkedBoats.Count == 1)
                        {
                            selectedSpace = space.SpaceId;
                            spaceFound = true;
                            break;
                        }
                    }
                }

                if (spaceFound == false)
                {
                    var q2 = harbour
                       .Where(h => h.ParkedBoats.Count == 0)
                       .FirstOrDefault(h => h.ParkedBoats.Count == 0);

                    selectedSpace = q2.SpaceId;
                }
            }
            return selectedSpace;
        }

        private static void AddBoatToHarbour(Boat boat, HarbourSpace[] harbour, int position)
        {
            harbour[position].ParkedBoats.Add(boat);
        }

        private static double ConvertToKmPerHour(int knot)
        {
            return knot * 1.852;
        }

        private static void SaveToFile(StreamWriter sw, HarbourSpace[] harbour)
        {
            int index = 0;
            foreach (var space in harbour)
            {
                if (space != null)
                {
                    foreach (Boat boat in space.ParkedBoats)
                    {
                        if (space.ParkedBoats != null)
                        {
                            sw.WriteLine(boat.TextToFile(index));  // Krasch: Cannot write to closed file
                        }
                    }
                }

                index++;
            }

            sw.Close();
        }

        private static void AddNewBoats(List<Boat> boats, int newBoats)
        {
            for (int i = 0; i < newBoats; i++)
            {
                int boatType = Utils.r.Next(4);

                switch (boatType)
                {
                    case 0:
                        CreateRowingBoat(boats);
                        break;
                    case 1:
                        CreateMotorBoat(boats);
                        break;
                    case 2:
                        CreateSailingBoat(boats);
                        break;
                    case 3:
                        CreateCargoShip(boats);
                        break;
                }
            }
        }

        private static void CreateRowingBoat(List<Boat> boats)
        {
            int weight = Utils.r.Next(100, 300 + 1);
            int maxSpeed = Utils.r.Next(3 + 1);
            int daysStaying = 1;
            int maxPassengers = Utils.r.Next(1, 6 + 1);
            int daysSinceArrival = 0;

            RowingBoat rowingBoat = new RowingBoat(weight, maxSpeed, daysStaying, daysSinceArrival, maxPassengers);
            boats.Add(rowingBoat);
        }

        private static void CreateMotorBoat(List<Boat> boats)
        {
            //string id = 
            int weight = Utils.r.Next(200, 3000 + 1);
            int maxSpeed = Utils.r.Next(60 + 1);
            int power = Utils.r.Next(10, 1000 + 1);
            int daysStaying = 3;
            int daysSinceArrival = 0;

            MotorBoat motorBoat = new MotorBoat(weight, maxSpeed, daysStaying, daysSinceArrival, power);
            boats.Add(motorBoat);
        }
        private static void CreateSailingBoat(List<Boat> boats)
        {
            int weight = Utils.r.Next(800, 6000 + 1);
            int maxSpeed = Utils.r.Next(12 + 1);
            int length = Utils.r.Next(10, 60 + 1);
            int daysStaying = 4;
            int daysSinceArrival = 0;

            SailingBoat sailingBoat = new SailingBoat(weight, maxSpeed, daysStaying, daysSinceArrival, length);
            boats.Add(sailingBoat);
        }
        private static void CreateCargoShip(List<Boat> boats)
        {
            int weight = Utils.r.Next(3000, 20000 + 1);
            int maxSpeed = Utils.r.Next(20 + 1);
            int containers = Utils.r.Next(500 + 1);
            int daysStaying = 6;
            int daysSinceArrival = 0;

            CargoShip cargoBoat = new CargoShip(weight, maxSpeed, daysStaying, daysSinceArrival, containers);
            boats.Add(cargoBoat);
        }
    }
}
