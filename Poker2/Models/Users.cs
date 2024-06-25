namespace Poker2.Models;

public class Users
{
    public Users(string UserName, Guid TableId, int UserMode)
    {
        userMode = UserMode;
        tableId = TableId;
        userMode = UserMode;
    }
    public Guid id { get; set; }
    public string userName { get; set; } = "";
    public Guid tableId { get; set; }
    public int userMode { get; set; }
}

public enum UserMode
{
    visitor = 0,
    standard = 1
}