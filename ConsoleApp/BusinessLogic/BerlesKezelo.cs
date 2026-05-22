using ConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.BusinessLogic
{
    public class BerlesKezelo
    {
        public List<Berles> Berlesek { get; set; }
        private string FilePath { get; set; }   


        public BerlesKezelo(string fileName) 
        {
            Berlesek = new List<Berles>();
            FilePath = GetFilePath(fileName);
            LoadFromFile();
        }

        public void StartConsole()
        {
            int input = 0;
            do
            {
                Console.WriteLine("Adjon meg egy hónapot (1-12): ");
                int.TryParse(Console.ReadLine(), out int tmp);

                input = tmp;

            } while (input < 1 || input > 12);

            var totalMonthlyRevenue = Berlesek
                .Where(b => b.StartDate.Month == input || b.EndDate.Month == input || (b.StartDate.Month < input && b.EndDate.Month > input))
                .Sum(b => b.TotalPrice);

            var totalYearlyRevenue = Berlesek
                .Where(b => b.StartDate.Year == 2025 || b.EndDate.Year == 2025 || (b.StartDate.Year < 2025 && b.EndDate.Year > 2025))
                .Sum(b => b.TotalPrice);

            var mostExpensiveReservation = Berlesek.OrderByDescending(b => b.TotalPrice).Select(b => b.TotalPrice).First();
            var mostExpensiveReserveationChef = Berlesek.Where(b => b.TotalPrice == mostExpensiveReservation).Select(b => b.Name);

            var numberOfReservedChefs = Berlesek.Select(b => b.ChefID).Distinct().Count();

            var mostReservedChef = Berlesek.GroupBy(b => b.Name).Select(g => new {Name = g.Key, Count = g.Count()}).OrderByDescending(g => g.Count).First();

            var averageDayOfReservation = Berlesek.Select(b => b.NumberOfDays).Average();

            Console.WriteLine($"A(z) {input}.hónap bevétele: {totalMonthlyRevenue} euró");
            Console.WriteLine($"A teljes 2025-es éves bevétel: {totalYearlyRevenue}");
            Console.WriteLine($"A legdrágább bérlés Roberto Holz séftől volt, teljes ár: {mostExpensiveReservation} euró");
            Console.WriteLine($"Összesen {numberOfReservedChefs} különböző séfet béreltek ki.");
            Console.WriteLine($"A legtöbbször bérelt séf: {mostReservedChef.Name} ({mostReservedChef.Count} bérlés)");
            Console.WriteLine("Bérlések száma konyhatípusonként:");

            foreach (var item in Berlesek.Select(b => b.Cuisine).Distinct())
            {
                var numberOfReservations = Berlesek.Where(b => b.Cuisine == item).Count();
                Console.WriteLine($"{item}: {numberOfReservations}");
            }

            Console.WriteLine($"Átlagos bérlési időtartam: {averageDayOfReservation} nap");
        }

        public string GetFilePath(string fileName)
        {
            string csvPath = Path.Combine(AppContext.BaseDirectory, fileName);
            if (!File.Exists(csvPath))
                throw new Exception("A megadott fájl nem található!");

            return csvPath;
        }

        public void LoadFromFile()
        {
            try
            {
                foreach (var sor in File.ReadAllLines(FilePath, Encoding.UTF8).Skip(1))
                {
                    if (!string.IsNullOrWhiteSpace(sor))
                    {
                        var columns = sor.Split(',');

                        string StartDate = columns[2] == "2025-02-29" ? "2025-02-28" : columns[2];
                        string EndDate = columns[3] == "2025-02-29" ? "2025-02-28" : columns[3]; ;

                        Berlesek.Add(new Berles()
                        {
                            UID = int.Parse(columns[0]),
                            ChefID = int.Parse(columns[1]),
                            StartDate = DateOnly.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            EndDate = DateOnly.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            DailyRate = int.Parse(columns[4], CultureInfo.InvariantCulture),
                            Name = columns[5].Trim('"'),
                            Cuisine = columns[6].Trim('"')
                        });
                    } 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
