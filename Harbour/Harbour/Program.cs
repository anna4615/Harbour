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

            var fileText = File.ReadLines("BoatsInHarbour.txt", System.Text.Encoding.UTF7);

            //Console.WriteLine("Text från fil");
            //foreach (var line in fileText)
            //{
            //    Console.WriteLine(line);
            //}
            //Console.WriteLine();

            HarbourSpace[] harbour = new HarbourSpace[64];

            for (int i = 0; i < harbour.Length; i++)
            {
                harbour[i] = new HarbourSpace(i);
            }

            AddBoatsFromFileToHarbour(fileText, harbour);

            Console.WriteLine("Båtar i hamn efter uppstart\n");
            Console.WriteLine(PrintHarbour(harbour));
            Console.WriteLine();

            bool goToNextDay = true;

            while (goToNextDay)
            {
                List<Boat> boatsInHarbour = GenerateBoatsInHarbourList(harbour);
                AddDayToDaysSinceArrival(boatsInHarbour);

                bool boatRemoved = true;

                while (boatRemoved)
                {
                    boatRemoved = RemoveBoats(harbour);
                }

                Console.WriteLine("Båtar i hamn efter dagens avfärder");
                Console.WriteLine(PrintHarbour(harbour));
                Console.WriteLine();

                int rejectedRowingBoats = 0;
                int rejectedMotorBoats = 0;
                int rejectedSailingBoats = 0;
                int rejectedCargoShips = 0;

                List<Boat> arrivingBoats = new List<Boat>();
                int NumberOfArrivingBoats = 10;

                CreateNewBoats(arrivingBoats, NumberOfArrivingBoats); // Tar bor tillfälligt, för att kunna styra vilka båtar som läggs till

                // Skapar båtar för test, ta bort sedan
                //arrivingBoats.Add(new MotorBoat("M-" + Boat.GenerateID(), 10, 2, 3, 0, 4));
                //arrivingBoats.Add(new RowingBoat("R-" + Boat.GenerateID(), 10, 2, 1, 0, 4));
                //arrivingBoats.Add(new SailingBoat("S-" + Boat.GenerateID(), 10, 2, 4, 0, 4));
                //arrivingBoats.Add(new CargoShip("L-" + Boat.GenerateID(), 10, 2, 6, 0, 4));
                //arrivingBoats.Add(new RowingBoat("R-" + Boat.GenerateID(), 10, 2, 1, 0, 4));

                Console.WriteLine("Anländande båtar");
                foreach (var boat in arrivingBoats)  //Kontroll, ta bort sedan
                {
                    Console.WriteLine(boat.ToString());
                }
                Console.WriteLine();

                foreach (var boat in arrivingBoats)
                {
                    int harbourPosition;
                    bool spaceFound;

                    if (boat is RowingBoat)
                    {
                        (harbourPosition, spaceFound) = RowingBoat.FindRowingboatSpace(harbour);

                        if (spaceFound)
                        {
                            harbour[harbourPosition].ParkedBoats.Add(boat);
                        }
                        else
                        {
                            rejectedRowingBoats++;
                        }
                    }

                    else if (boat is MotorBoat)
                    {
                        (harbourPosition, spaceFound) = MotorBoat.FindMotorBoatSpace(harbour);

                        if (spaceFound)
                        {
                            harbour[harbourPosition].ParkedBoats.Add(boat); // om harbpos är 1 och harbourpos[0] är tom hamnar båten både på 0 och 1!!!!
                        }
                        else
                        {
                            rejectedMotorBoats++;
                        }
                    }

                    else if (boat is SailingBoat)
                    {
                        (harbourPosition, spaceFound) = SailingBoat.FindSailingBoatSpace(harbour);

                        if (spaceFound)
                        {
                            harbour[harbourPosition].ParkedBoats.Add(boat);
                            harbour[harbourPosition + 1].ParkedBoats = harbour[harbourPosition].ParkedBoats;
                        }

                        if (spaceFound == false)
                        {
                            rejectedSailingBoats++;
                        }
                    }

                    else if (boat is CargoShip)
                    {
                        (harbourPosition, spaceFound) = CargoShip.FindCargoShipSpace(harbour);

                        if (spaceFound)
                        {
                            harbour[harbourPosition].ParkedBoats.Add(boat);
                            harbour[harbourPosition + 1].ParkedBoats = harbour[harbourPosition].ParkedBoats;
                            harbour[harbourPosition + 2].ParkedBoats = harbour[harbourPosition].ParkedBoats;
                            harbour[harbourPosition + 3].ParkedBoats = harbour[harbourPosition].ParkedBoats;
                        }

                        if (spaceFound == false)
                        {
                            rejectedCargoShips++;
                        }
                    }
                }

                Console.WriteLine("Båtar i hamn vid dagens slut:");
                Console.WriteLine(PrintHarbour2(harbour));
                Console.WriteLine();

                boatsInHarbour = GenerateBoatsInHarbourList(harbour);

                Console.WriteLine(GnerateSummaryOfBoats(boatsInHarbour));

                int sumOfWeight = GenerateSumOfWeight(boatsInHarbour);
                double averageSpeed = GenerateAverageSpeed(boatsInHarbour);
                int availableSpaces = CountAvailableSpaces(harbour);

                Console.WriteLine(PrintStatistics(sumOfWeight, averageSpeed, availableSpaces,
                    rejectedRowingBoats, rejectedMotorBoats, rejectedSailingBoats, rejectedCargoShips));

                Console.WriteLine();
                Console.WriteLine();

                Console.Write("Tryck \"j\" för att gå till nästa dag eller valfri annan tangent för att avsluta ");
                string input = Console.ReadLine();

                goToNextDay = input == "j";

                Console.WriteLine();
                Console.WriteLine();
            }


            StreamWriter sw = new StreamWriter("BoatsInHarbour.txt", false, System.Text.Encoding.UTF7);

            SaveToFile(sw, harbour);
            sw.Close();

        }

        private static string PrintStatistics(int sumOfWeight, double averageSpeed, int availableSpaces,
            int rejectedRowingBoats, int rejectedMotorBoats, int rejectedSailingBoats, int rejectedCargoShips)
        {
            return $"Statistik\n---------\n" +
                $"Total båtvikt i hamn:\t{sumOfWeight} kg\n" +
                $"Medel av maxhastighet:\t{ConvertToKmPerHour(averageSpeed)} km/h\n" +
                $"Lediga platser:\t\t{availableSpaces} st\n\n" +
                $"Avvisade båtar:\n" +
                $"\tRoddbåtar\t{rejectedRowingBoats} st\n" +
                $"\tMotorbåtar\t{rejectedMotorBoats} st\n" +
                $"\tSegelbåtar\t{rejectedSailingBoats} st\n" +
                $"\tLastfartyg\t{rejectedCargoShips} st\n" +
                $"\tTotalt\t\t{rejectedRowingBoats + rejectedMotorBoats + rejectedSailingBoats + rejectedCargoShips} st";
        }

        private static int CountAvailableSpaces(HarbourSpace[] harbour)
        {
            var q = harbour
                .Where(s => s.ParkedBoats.Count() == 0);

            return q.Count();
        }

        private static int GenerateSumOfWeight(List<Boat> boatsInHarbour)
        {
            var q = boatsInHarbour
                .Select(b => b.Weight)
                .Sum();

            return q;
        }

        private static double GenerateAverageSpeed(List<Boat> boatsInHarbour)
        {
            var q = boatsInHarbour
                .Select(b => b.MaximumSpeed)
                .Average();

            return q;
        }

        private static string GnerateSummaryOfBoats(List<Boat> boatsInHarbour)
        {
            string summaryOfBoats = "Båtar i hamn\n------------\n";

            var q = boatsInHarbour
                    .GroupBy(b => b.Type)
                    .OrderBy(g => g.Key);

            foreach (var group in q)
            {
                summaryOfBoats += $"{group.Key}:\t{group.Count()} st\n";
            }

            return summaryOfBoats;
        }

        private static bool RemoveBoats(HarbourSpace[] harbour)
        {
            bool boatRemoved = false;

            foreach (HarbourSpace space in harbour)
            {
                foreach (Boat boat in space.ParkedBoats)
                {
                    if (boat.DaysSinceArrival == boat.DaysStaying)
                    {
                        space.ParkedBoats.Remove(boat);
                        boatRemoved = true;
                        break;
                    }
                }
                if (boatRemoved)
                {
                    break;
                }
            }

            return boatRemoved;
        }

        private static List<Boat> GenerateBoatsInHarbourList(HarbourSpace[] harbour)
        {

            // Större båtar finns på flera platser i harbour, gör lista med endast en kopia av vardera båt
            var q1 = harbour
                .Where(h => h.ParkedBoats.Count != 0);

            List<Boat> allCopies = new List<Boat>();

            foreach (var space in q1)
            {
                foreach (var boat in space.ParkedBoats)
                {
                    allCopies.Add(boat); // Innehåller kopior
                }
            }

            var q2 = allCopies
                .GroupBy(b => b.IdNumber);

            List<Boat> singleBoats = new List<Boat>();

            foreach (var group in q2)
            {
                var q = group
                    .FirstOrDefault();

                singleBoats.Add(q);  // Lista utan kopior
            }

            return singleBoats;
        }

        private static void AddDayToDaysSinceArrival(List<Boat> boats)
        {
            foreach (var boat in boats)
            {
                boat.DaysSinceArrival++;
            }
        }
        private static string PrintHarbour(HarbourSpace[] harbour)
        {
            string text = "Båtplats\tBåttyp\t\tID\tVikt (kg)\tMaxhastighet (km/h)\tÖvrigt\n" +
                          "----------\t----------\t-----\t---------\t-------------------\t------------------------------\n";

            foreach (var space in harbour)
            {
                if (space.ParkedBoats.Count() == 0)
                {
                    text += $"{space.SpaceId}  Ledigt\n";
                }
                foreach (var boat in space.ParkedBoats)
                {
                    text += $"{space.SpaceId}\t\t{boat}\n";
                }
            }

            return text;
        }
        private static string PrintHarbour2(HarbourSpace[] harbour)
        {
            string text = "Båtplats\tBåttyp\t\tID\tVikt\tMaxhastighet\tÖvrigt\n" +
                          "        \t      \t\t  \t(kg)\t(km/h)\n" +
                          "--------\t----------\t-----\t-----\t------------\t------------------------------\n";

            foreach (var space in harbour)
            {
                if (space.ParkedBoats.Count() == 0)
                {
                    text += $"   {space.SpaceId}  Ledigt\n";
                }
                foreach (var boat in space.ParkedBoats)
                {
                    if (space.SpaceId > 0 && harbour[space.SpaceId - 1].ParkedBoats.Contains(boat))
                    {
                        if (boat is SailingBoat)
                        {
                            text += $"   {space.SpaceId}\t\t||||||||\n";

                        }
                        if (boat is CargoShip)
                        {
                            text += $"   {space.SpaceId}\t\t||||||||||\n";
                        }
                    }
                    else
                    {
                        text += $"   {space.SpaceId}\t\t{boat}\n";
                    }
                }
            }
            return text;
        }

        private static void AddBoatsFromFileToHarbour(IEnumerable<string> fileText, HarbourSpace[] harbour)
        {
            // File:
            // 0:index; 1:Id; 2:Weight; 3:MaxSpeed; 4:Type; 5:DaysStaying; 6:DaySinceArrival; 7:Special

            foreach (var line in fileText)
            {
                int index;
                string[] boatData = line.Split(";");

                switch (boatData[4])
                {
                    case "Roddbåt":
                        index = int.Parse(boatData[0]);
                        harbour[index].ParkedBoats.Add
                            (new RowingBoat(boatData[1], int.Parse(boatData[2]), int.Parse(boatData[3]),
                            int.Parse(boatData[5]), int.Parse(boatData[6]), int.Parse(boatData[7])));
                        break;

                    case "Motorbåt":
                        index = int.Parse(boatData[0]);
                        harbour[index].ParkedBoats.Add
                            (new MotorBoat(boatData[1], int.Parse(boatData[2]), int.Parse(boatData[3]),
                            int.Parse(boatData[5]), int.Parse(boatData[6]), int.Parse(boatData[7])));
                        break;

                    case "Segelbåt":
                        index = int.Parse(boatData[0]);

                        if (harbour[index].ParkedBoats.Count == 0) // När andra halvan av segelbåten kommmer från foreach är den redan tillagd på den platsen annars hade det blivit två kopior av samma båt
                        {
                            harbour[index].ParkedBoats.Add
                                (new SailingBoat(boatData[1], int.Parse(boatData[2]), int.Parse(boatData[3]),
                                int.Parse(boatData[5]), int.Parse(boatData[6]), int.Parse(boatData[7])));

                            harbour[index + 1].ParkedBoats = harbour[index].ParkedBoats; // samma båt på två platser
                        }
                        break;

                    case "Lastfartyg":
                        index = int.Parse(boatData[0]);

                        if (harbour[index].ParkedBoats.Count == 0) // När resten av lastfartyget kommmer från foreach är det redan tillagt, annars hade det blivit kopior
                        {
                            harbour[index].ParkedBoats.Add
                            (new CargoShip(boatData[1], int.Parse(boatData[2]), int.Parse(boatData[3]),
                            int.Parse(boatData[5]), int.Parse(boatData[6]), int.Parse(boatData[7])));

                            harbour[index + 1].ParkedBoats = harbour[index].ParkedBoats;
                            harbour[index + 2].ParkedBoats = harbour[index].ParkedBoats;
                            harbour[index + 3].ParkedBoats = harbour[index].ParkedBoats;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static double ConvertToKmPerHour(double knot)
        {
            return Math.Round(knot * 1.852, 0);
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
                            sw.WriteLine(boat.TextToFile(index), System.Text.Encoding.UTF7);
                        }
                    }
                }

                index++;
            }

            sw.Close();
        }

        private static void CreateNewBoats(List<Boat> boats, int newBoats)
        {
            for (int i = 0; i < newBoats; i++)
            {
                int boatType = Utils.r.Next(4);

                switch (boatType)
                {
                    case 0:
                        RowingBoat.CreateRowingBoat(boats);
                        break;
                    case 1:
                        MotorBoat.CreateMotorBoat(boats);
                        break;
                    case 2:
                        SailingBoat.CreateSailingBoat(boats);
                        break;
                    case 3:
                        CargoShip.CreateCargoShip(boats);
                        break;
                }
            }
        }
    }
}
