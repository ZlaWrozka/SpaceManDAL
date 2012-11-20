using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceManConnectedLayer
{
    public class Room
    {
        public DateTime AddedTime { get; set; }
        public int ID { get; set; }
        public string PetName { get; set; }
        public short SeatsNumber { get; set; }
        public short StandsNumber { get; set; }
        public byte Storey { get; set; }
        public bool HasProjector { get; set; }
        public string Description { get; set; }
        public int MP_ID { get; set; }
    }

    public class ManagementPlan
    {
        private int id = 0;
        public int ID
        {
            get { return id; }
        }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
    }

    public class Reservation
    {
        private int id = 0;
        public int ID { get { return id; } }
        public string Person { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
}
