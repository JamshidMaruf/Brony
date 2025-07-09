namespace Brony.Domain;

public class Hasanxon
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Occupation { get; set; }
    public Hasanxon(string name, int age, string occupation)
    {
        Name = name;
        Age = age;
        Occupation = occupation;
    }
    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}, Occupation: {Occupation}");
    }
}
