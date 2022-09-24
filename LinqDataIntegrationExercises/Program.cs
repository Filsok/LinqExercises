using LinqDataIntegrationExercises;
using Newtonsoft.Json;


internal class Program
{
    private static void Main(string[] args)
    {
        var people = LoadPeople();
        var addressess = LoadAddressess();
    }

    private static object LoadAddressess()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, "Person/Addressess.json");

        var json = File.ReadAllText(jsonFullPath);

        return JsonConvert.DeserializeObject<List<Person>>(json);
    }

    private static object LoadPeople()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, "Person/People.json");

        var json = File.ReadAllText(jsonFullPath);

        return JsonConvert.DeserializeObject<List<Person>>(json);
    }
}