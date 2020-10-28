using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class HarbourSpace
    {
        public int SpaceId { get; set; }
        //public int DaysSinceArrival { get; set; }
        public List<Boat> ParkedBoats { get; set; }


        public HarbourSpace(int id)
        {
            SpaceId = id;
            //DaysSinceArrival = 0;
            ParkedBoats = new List<Boat>();
        }

        public static void AddBoatToHarbour(Boat boat, HarbourSpace[] harbour, int position)
        {
            harbour[position].ParkedBoats.Add(boat);
        }
    }
}
