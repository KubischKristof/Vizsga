using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public class Koltseg
    {
        public int Id { get; set; }
        public string ChefName { get; set; } = "";
        public DateOnly Datum { get; set; }
        public string Kategoria { get; set; } = "";
        public decimal Osszeg { get; set; }
        public string Megjegyzes { get; set; } = "";

        public Koltseg()
        { 
        }
    }
}
