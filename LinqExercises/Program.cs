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
using System.Threading.Tasks.Dataflow;

/*
Getting Data:
.Where(predicate)
.Single(predicate)      .SingleOrDefault(predicate)
.First(predicate)       .FirstOrDefault(predicate)
.Last(predicate)        .LastOrDefault(predicate)

Data projection:
.Select(selector)       .SelectMany(selector)  //flatting

Data divide:
.Take()                 .TakeLast()                     .TakeWhile(predicate)
.Skip()                 .SkipLast()                     .SkipWhile(predicate)

Data sort:
.OrderBy(keySelector)   .OrderByDescending(keySelector)
.ThenBy(keySelector)    .ThenByDescending(keySelector)

Data set operations:
.Distinct()             .DistinctBy()
.Union(set)             .UnionBy(set)
.Intersect(set)         .IntersectBy(set)
.Except(set)            .ExceptBy(set)

Data verification:
.All(predicate)
.Any(predicate)

Data segregation:
.GroupBy(keySelector)

Operation on groups:
.


 */

class Program
{
    static void Main(string[] args)
    {
        var csvPath = @"D:\git\LinqExercises\LinqExercises\LinqExercises\googleplaystore1.csv";
        var googleApps = LoadGoogleApps(csvPath);

        //Display(googleApps);
        //GetData(googleApps);
        //ProjecitionData(googleApps);
        //DivideData(googleApps);
        //OrderData(googleApps);
        //DataSetOperation(googleApps);
        //DataVerification(googleApps);
        //GroupData(googleApps);
        GroupDataOperations(googleApps);
    }

    private static void GroupDataOperations(List<GoogleApp> googleApps)
    {
        var categoryGroup = googleApps
            .GroupBy(a => a.Category)
            //.Where(g => g.Min(a => a.Reviews >= 10))
            ;

        foreach (var group in categoryGroup)
        {
            var averageReviews = Math.Round(group.Average(a => a.Reviews));
            var minReviews = group.Min(a => a.Reviews);
            var maxReviews = group.Max(a => a.Reviews);

            var reviewsCount = group.Sum(a => a.Reviews);

            var allAppsFromGroupHaveRatingOfTree = group.All(a => a.Rating > (decimal)3.0);

            Console.WriteLine($"Category: {group.Key}");
            Console.WriteLine($"avarageReviews: {averageReviews}");
            Console.WriteLine($"minReviews: {minReviews}");
            Console.WriteLine($"maxReviews: {maxReviews}");
            Console.WriteLine($"reviewsCount: {reviewsCount}");
            Console.WriteLine($"allAppsFromGroupHaveRatingOfTree: {allAppsFromGroupHaveRatingOfTree}");
            Console.WriteLine("\n");
        }
    }

    private static void GroupData(List<GoogleApp> googleApps)
    {
        //var categoryGroup = googleApps.GroupBy(a => a.Category);
        var categoryGroup = googleApps.GroupBy(a => new { a.Category, a.Type });

        //var artAndDesignGroup = categoryGroup.First(g => g.Key == Category.ART_AND_DESIGN);
        ////var appsArtAndDesignGroup = artAndDesignGroup.Select(e => e);          //first method
        //var appsArtAndDesignGroup = artAndDesignGroup.ToList();                  //second method
        //Console.WriteLine($"Displaying elements for group {artAndDesignGroup.Key}");
        //Display(appsArtAndDesignGroup);

        foreach (var group in categoryGroup)
        {
            var apps = group.ToList();
            //Console.WriteLine($"\n\nDisplaying elements for group _______{group.Key}_______");
            var averageReviews = group.Average(g => g.Reviews);
            Console.WriteLine($"\n\nDisplaying elements for group _______{group.Key.Category} , {group.Key.Type}_______");
            Display(apps);
        }
    }

    static void DataVerification(List<GoogleApp> googleApps)
    {
        var allOperatorResult = googleApps.Where(a => a.Category == Category.WEATHER)
            .All(a => a.Reviews > 10);
        Console.WriteLine($"Result allOperatorResult: {allOperatorResult}");

        var anyOperatorResult = googleApps.Where(a => a.Category == Category.WEATHER)
            .Any(a => a.Reviews > 2_000_000);
        Console.WriteLine($"Result anyOperatorResult: {anyOperatorResult}");
    }

    static void DataSetOperation(List<GoogleApp> googleApps)
    {
        //var paidAppsCategories = googleApps.Where(a => a.Type == LinqExercises.Type.Paid)
        //    .Select(a=>a.Category).Distinct();
        //Console.WriteLine($"Paid apps categories: {string.Join(", ", paidAppsCategories)}");

        var setA = googleApps.Where(a => a.Rating > (decimal)4.7 && a.Type == LinqExercises.Type.Paid && a.Reviews > 1000)
            .DistinctBy(a => a.Name);
        var setB = googleApps.Where(a => a.Name.Contains("Pro") && a.Rating > (decimal)4.6 && a.Reviews > 10000)
            .DistinctBy(a => a.Name);

        Console.WriteLine("\nApps Union:");
        var appsUnion = setA.Union(setB);
        Display(appsUnion);

        Console.WriteLine("\nApps Intersect:");
        var appsIntersect = setA.Intersect(setB);
        Display(appsIntersect);

        Console.WriteLine("\nApps Except:");
        var appsExcept = setA.Except(setB);
        Display(appsExcept);
    }

    static void OrderData(List<GoogleApp> googleApps)
    {
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.4 && app.Category == Category.BEAUTY);

        var sortedResults = highRatedBeautyApps
            .OrderByDescending(app => app.Rating)
            .ThenByDescending(app => app.Reviews)
            .Take(8);
        Display(sortedResults);
    }

    static void DivideData(IEnumerable<GoogleApp> googleApps)
    {
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.4 && app.Category == Category.BEAUTY);
        //var first5HighRatedBeautyApps = highRatedBeautyApps.Take(5);
        //var first5HighRatedBeautyApps = highRatedBeautyApps.TakeLast(5);
        //var first5HighRatedBeautyApps = highRatedBeautyApps.TakeWhile(app=>app.Reviews>1000).Take(5);
        //Display(first5HighRatedBeautyApps);

        //var skippedResults = highRatedBeautyApps.Skip(5);
        //var skippedResults = highRatedBeautyApps.SkipLast(5);
        var skippedResults = highRatedBeautyApps.SkipWhile(app => app.Reviews > 1000);
        Display(skippedResults);
    }

    static void ProjecitionData(IEnumerable<GoogleApp> googleApps)
    {
        var highRatedBeautyApps = googleApps.Where(app => app.Rating > (decimal)4.6 && app.Category == Category.BEAUTY);

        var highRatedBeautyAppsNames = highRatedBeautyApps.Select(app => app.Name);

        var dtos = highRatedBeautyApps.Select(app => new GoogleAppDto()
        { Reviews = app.Reviews, Name = app.Name });

        var annonymousDtos = highRatedBeautyApps.Select(app => new
        { Reviews = app.Reviews, Name = app.Name });

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
        var FirstHighRatedBeautyApp = highRatedBeautyApps.LastOrDefault(a => a.Reviews < 300);
        Console.WriteLine($"High rated beauty app is: ");
        Console.WriteLine(FirstHighRatedBeautyApp);
    }

    static void Display(IEnumerable<GoogleApp> googleApps)
    {
        foreach (var googleApp in googleApps)
        {
            Console.WriteLine(googleApp);
        }
        Console.WriteLine($"Rows count: {googleApps.Count()}");
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