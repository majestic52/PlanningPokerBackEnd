using Poker2.Models;

namespace Poker2.Contracts;

public record CreateTableRequest(string NameTable, Points Points);