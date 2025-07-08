namespace Brony.Domain;

public class Student
{
    public string Name { get; set; }
    public List<int> Scores { get; set; }

    public Student()
    {
        Scores = new List<int>() { 2, 4, 8, 1, 5, 7, 3, 6, 10 };
    }

    // Output: High = 10, Middle = 5, Low = 1
    public (double High, double Middle, double Low) GetScores()
    {
        return (Scores.Max(), Scores.Min(), Scores.Average());
    }
}