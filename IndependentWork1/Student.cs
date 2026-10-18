namespace IndependentWork1;

public class Student
{
    // Приватні поля
    private string _fullName;
    private double _averageScore;

    // Властивості
    public string FullName
    {
        get { return _fullName; }
    }

    public double AverageScore
    {
        get { return _averageScore; }
        set { _averageScore = value; }
    }

    // Конструктор
    public Student(string fullName, double averageScore)
    {
        _fullName = fullName;
        _averageScore = averageScore;
    }

    //перевірка, чи є студент відмінником
    public bool IsExcellentStudent()
    {
        return _averageScore >= 4.5;
    }
}