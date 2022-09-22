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

/*
Getting Data:
.Where(predicate)
.Single(predicate)      .SingleOrDefault(predicate)
.First(predicate)       .FirstOrDefault(predicate)
.Last(predicate)        .LastOrDefault(predicate)
.OrderBy(keySelector)   .OrderByDescending(keySelector)

Data projection:
.Select(selector)       .SelectMany(selector)  //flatting



 */

class Program
{
    static void Main(string[] args)
    {
        var csvPath = @"D:\git\LinqExercises\LinqExercises\LinqExercises\googleplaystore1.csv";
        var googleApps = LoadGoogleApps(csvPath);

        //Display(googleApps);
        ProjecitionData(googleApps);
    }

    static void ProjecitionData(IEnumerable<GoogleApp> googleApps)
    {
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.6 && app.Category == Category.BEAUTY);

        var highRatedBeautyAppsNames =highRatedBeautyApps.Select(app=>app.Name);

        var dtos = highRatedBeautyApps.Select(app=>new GoogleAppDto() 
            { Reviews=app.Reviews, Name=app.Name});
        
        var annonymousDtos = highRatedBeautyApps.Select(app=>new 
            { Reviews=app.Reviews, Name=app.Name});

        foreach (var dto in annonymousDtos)
            Console.WriteLine($"{dto.Name}: {dto.Reviews}");


        //var genres = highRatedBeautyApps.SelectMany(app => app.Genres);
        //Console.WriteLine(String.Join(", ", genres));
    }

    static void GetData(IEnumerable<GoogleApp> googleApps)
    {
        var highRatedApps = googleApps.Where(app => app.Rating > (decimal)4.6);
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.6 && app.Category == Category.BEAUTY);
        Display(highRatedBeautyApps);
        
        //var FirstHighRatedBeautyApp = highRatedBeautyApps.OrderByDescending(a=> a.Rating).First();
        //var FirstHighRatedBeautyApp = highRatedBeautyApps.First(a=>a.Reviews < 300);
        //var FirstHighRatedBeautyApp = highRatedBeautyApps.SingleOrDefault(a=>a.Reviews < 50);
        var FirstHighRatedBeautyApp = highRatedBeautyApps.LastOrDefault(a=>a.Reviews < 300);
        Console.WriteLine($"High rated beauty app is: ");
        Console.WriteLine(FirstHighRatedBeautyApp);
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