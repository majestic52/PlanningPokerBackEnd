namespace Poker2.Contracts;

public record GetTablesRequest(string? Search, string? SortItem, string? SortOrder);