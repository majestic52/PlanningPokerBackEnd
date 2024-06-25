namespace Poker2.Contracts;

public record CreateUserRequest(string UserName, Guid TableId, int UserMode);