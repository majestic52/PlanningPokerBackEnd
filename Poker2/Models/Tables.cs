namespace Poker2.Models;

public class Tables
{
    public Tables(string NameTable, Points Points)
    {
        nameTable = NameTable;
        points = Points;
    }
    
    public Guid id { get; set; }
    public string nameTable { get; set; }
    public Points points { get; set; }
}

public enum Points
{
    FIBONACCI,
    ONE_TO_TEN,
    TSHIRTS,
    POWERS_OF_TWO
}