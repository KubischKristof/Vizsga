using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Model
{
    public class Berles
    {
        public int UID { get; set; }
        public int ChefID { get; set; }
        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; }
        public int DailyRate { get; set; }
        public string Name { get; set; }
        public string Cuisine { get; set; }

        public int NumberOfDays => (EndDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
        public int TotalPrice => NumberOfDays * DailyRate;


        public Berles()
        {
        }

        public Berles(int uid, int chefid, DateOnly startdate, DateOnly enddate, int daily_rate, string name, string cuisine)
        {
            this.UID = uid;
            this.ChefID = chefid;
            this.StartDate = startdate;
            this.EndDate = enddate;
            this.DailyRate = daily_rate;
            this.Name = name;
            this.Cuisine = cuisine;
        }
    }
}
