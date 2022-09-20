using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using LinqExercises;

class Program
{
    static void Main(string[] args)
    {
        var csvPath = @"D:\git\LinqExercises\LinqExercises\LinqExercises\googleplaystore1.csv";
        var googleApps = LoadGoogleApps(csvPath);

        //Display(googleApps);
        GetData(googleApps);
    }

    static void GetData(IEnumerable<GoogleApp> googleApps)
    {
        var highRatedApps = googleApps.Where(app => app.Rating > (decimal)4.6);
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.6 && app.Category == Category.BEAUTY);
        Display(highRatedBeautyApps);
    }

    static void Display(IEnumerable<GoogleApp> googleApps)
    {
        foreach (var googleApp in googleApps)
        {
            Console.WriteLine(googleApp);
        }
        Console.WriteLine($"Rows number: {googleApps.Count()}");
    }

    static void Display(GoogleApp googleApp)
    {
        Console.WriteLine(googleApp);
    }

    static List<GoogleApp> LoadGoogleApps(string csvPath)
    {
        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<GoogleAppMap>();
            var records = csv.GetRecords<GoogleApp>().ToList();
            return records;
        }
    }
}