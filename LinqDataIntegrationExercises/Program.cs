using System.Globalization;
using LinqDataIntegrationExercises;
using Newtonsoft.Json;


internal class Program
{
    private static void Main(string[] args)
    {
        var people = LoadPeople();
        var addresses = LoadAddressess();

        //JoinExercise(people, addresses);

        LeftJoinExercise(people, addresses);

        Console.WriteLine("");
    }


    private static void LeftJoinExercise(List<Person> people, List<Address> addresses)
    {
        var joinData = people.GroupJoin(addresses, p => p.Id, a => a.PersonId, (person, addresses) => new { person.Name, addresses });

    }

    private static void JoinExercise(List<Person> people, List<Address> addresses)
    {
        //var joinData = people.Join(addresses, p=>p.Id, a=>a.PersonId, 
        //    (person, address) => new { person.Naddressme, address.City, address.Street});
        var joinData = people.GroupJoin(addresses, p => p.Id, a => a.PersonId,
            (person, addresses) => new { person.Name, Adresses = addresses });


        //foreach (var element in joinData)
        //    Console.WriteLine($"Name: {element.Name}, address: {element.City} {element.Street}");

        foreach (var element in joinData)
        {
            Console.WriteLine($"Name: {element.Name}");
            foreach (var address in element.Adresses)
            {
                Console.WriteLine($"\tCity: {address.City}, street: {address.Street}");
            }
        }
    }

    private static List<Address> LoadAddressess()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, "Person/Addresses.json");

        var json = File.ReadAllText(jsonFullPath);

        return JsonConvert.DeserializeObject<List<Address>>(json);
    }

    private static List<Person> LoadPeople()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var jsonFullPath = Path.GetRelativePath(currentDir, "Person/People.json");

        var json = File.ReadAllText(jsonFullPath);

        return JsonConvert.DeserializeObject<List<Person>>(json);
    }
}