using System;
using System.Collections.Generic;
using System.IO;

namespace Harbour
{
    class Program
    {
        static Random r = new Random();

        static void Main(string[] args)
        {
            string fileText = File.ReadAllText("BoatsInHarbour.txt");
            Console.WriteLine(fileText);

            Boat[,] harbour = new Boat[64, 2];
            List<Boat> boats = new List<Boat>();
            int newBoats = 5;
            StreamWriter sw = new StreamWriter("BoatsInHarbour.txt", false);

            AddBoatsToList(boats, newBoats);
            SaveToFile(sw, boats);

            fileText = File.ReadAllText("BoatsInHarbour.txt");
            Console.WriteLine(fileText);

            foreach (var boat in boats)
            {
                Console.WriteLine(boat.ToString());
            }
        }

        private static void SaveToFile(StreamWriter sw, List<Boat> boats)
        {
            foreach (var boat in boats)
            {
                sw.WriteLine(boat.TextToFile());
            }
            sw.Close();
        }

        private static void AddBoatsToList(List<Boat> boats, int newBoats)
        {
            for (int i = 0; i < newBoats; i++)
            {
                int boatType = r.Next(4);

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
            int weight = r.Next(100, 300 + 1);
            int maxSpeed = r.Next(3 + 1);
            int maxPassengers = r.Next(1, 6 + 1);
            int daysInHarbour = 1;

            RowingBoat rowingBoat = new RowingBoat(weight, maxSpeed, daysInHarbour, maxPassengers);
            boats.Add(rowingBoat);
        }

        private static void CreateMotorBoat(List<Boat> boats)
        {
            int weight = r.Next(200, 3000 + 1);
            int maxSpeed = r.Next(60 + 1);
            int power = r.Next(10, 1000 + 1);
            int daysInHarbour = 3;

            MotorBoat motorBoat = new MotorBoat(weight, maxSpeed, daysInHarbour, power);
            boats.Add(motorBoat);
        }
        private static void CreateSailingBoat(List<Boat> boats)
        {
            int weight = r.Next(800, 6000 + 1);
            int maxSpeed = r.Next(12 + 1);
            int length = r.Next(10, 60 + 1);
            int daysInHarbour = 4;

            SailingBoat sailingBoat = new SailingBoat(weight, maxSpeed, daysInHarbour, length);
            boats.Add(sailingBoat);
        }
        private static void CreateCargoShip(List<Boat> boats)
        {
            int weight = r.Next(3000, 20000 + 1);
            int maxSpeed = r.Next(20 + 1);
            int containers = r.Next(500 + 1);
            int daysInHarbour = 6;

            CargoShip cargoBoat = new CargoShip(weight, maxSpeed, daysInHarbour, containers);
            boats.Add(cargoBoat);
        }
    }
}
