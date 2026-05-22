using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Model;

namespace WpfApp.BusinessLogic
{
    public class KoltsegKezelo
    {
        private string FilePath { get; set; }

        public List<Koltseg> Koltsegek { get; set; }

        public KoltsegKezelo(string fileName)
        {
            FilePath = Path.Combine(AppContext.BaseDirectory, fileName);

            Koltsegek = new List<Koltseg>();

            LoadFromFile();
        }

        public void WriteToFile()
        {
            var sorok = new List<string> { "id;chefname;datum;kategoria;osszeg;megjegyzes" };
            sorok.AddRange(Koltsegek.Select(k => $"{k.Id};{k.ChefName};{k.Datum:yyyy-MM-dd};{k.Kategoria};{k.Osszeg};{k.Megjegyzes}"));
            File.WriteAllLines(FilePath, sorok, Encoding.UTF8);
        }

        

        public void LoadFromFile()
        {
            Koltsegek.Clear();

            if (!File.Exists(FilePath)) return;
            foreach (var sor in File.ReadAllLines(FilePath, Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(sor)) continue;
                var columns = sor.Split(';');

                string Date = columns[2] == "2025-02-29" ? "2025-02-28" : columns[2];

                Koltsegek.Add(new Koltseg
                {
                    Id = int.Parse(columns[0]),
                    ChefName = columns[1],
                    Datum = DateOnly.ParseExact(Date, "yyyy-MM-dd"),
                    Kategoria = columns[3],
                    Osszeg = decimal.Parse(columns[4]),
                    Megjegyzes = columns.Length > 5 ? columns[5] : ""
                });
            }
        }
    }
}
