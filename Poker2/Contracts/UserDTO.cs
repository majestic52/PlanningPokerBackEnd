namespace Poker2.Contracts;

public record UserDTO(Guid Id, string UserName, Guid TableId, int UserMode);