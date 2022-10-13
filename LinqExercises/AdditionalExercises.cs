namespace LinqExercises
{
    public static class AdditionalExercises
    {
        public static void LikeMain(List<GoogleApp> googleApps)
        {
            //GettingData(googleApps);
            DataProjection(googleApps);
            //DataDivide(googleApps);
            //SortData(googleApps);
            //DataSetOperations(googleApps);
            //DataVerification(googleApps);
            //DataSegregation(googleApps);
            //GroupsOperations(googleApps);
        }

        private static void DataProjection(List<GoogleApp> googleApps)
        {
            //Data projection:
            //.Select(selector)     .SelectMany(selector)  //flatting


        }

        private static void GettingData(List<GoogleApp> googleApps)
        {
            //Getting Data:
            //.Where(predicate)
            //.Single(predicate)    .SingleOrDefault(predicate)
            //.First(predicate)     .FirstOrDefault(predicate)
            //.Last(predicate)      .LastOrDefault(predicate)

            var expensiveApps = googleApps
                .Where(a => Convert.ToDouble(a.Price.Trim('$')) > 10.0);
            //DisplayClass.Display(expensiveApps);

            var heavyApps = googleApps
                .Where(a => !a.Size.Contains("Varies with device") && !a.Size.Contains('k'))
                .Where(a => Convert.ToDouble(a.Size.TrimEnd('M')) > 90.0);
            //DisplayClass.Display(heavyApps);

            var heaviestExpensiveApp = googleApps
                .SingleOrDefault(a => a.Size == "100M" && Convert.ToDouble(a.Price.TrimStart('$')) > 10.0);
            //DisplayClass.Display(heaviestExpensiveApp);

            var mostExpensiveApp = googleApps
                .Where(a => a.Type == Type.Paid)
                .OrderByDescending(a => Convert.ToDouble(a.Price.TrimStart('$')))
                .FirstOrDefault(a => Convert.ToDouble(a.Price.TrimStart('$')) > 10.0);
            //DisplayClass.Display(mostExpensiveApp);

            var cheapestApp = googleApps
                .Where(a => a.Type == Type.Paid)
                .OrderByDescending(a => Convert.ToDouble(a.Price.TrimStart('$')))
                .LastOrDefault(a => Convert.ToDouble(a.Price.TrimStart('$')) < 1.0);
            DisplayClass.Display(cheapestApp);


        }
    }
}