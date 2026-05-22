using ConsoleApp.BusinessLogic;

class Program
{
    static void Main()
    {
        var fileName = "chef_berlesek_2025.csv";

        var berlesKezelo = new BerlesKezelo(fileName);

        berlesKezelo.StartConsole();
    }
}