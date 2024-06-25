namespace Poker2.Contracts;

public record GetUsersRequest(string? Search, string? SortItem, string? SortOrder);