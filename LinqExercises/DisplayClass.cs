using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
    public static class DisplayClass
    {
        public static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }
            Console.WriteLine($"Rows count: {googleApps.Count()}");
        }

        public static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }
    }
}
