using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poker2.Contracts;
using Poker2.DataAccess;
using Poker2.Models;

namespace Poker2.Controllers;

[ApiController]
[Route("[controller]")]
public class TableController : ControllerBase
{
    private readonly TableDBContext _context;

    public TableController(TableDBContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody]CreateTableRequest request, CancellationToken cancellationToken)
    {
        var table = new Tables(request.NameTable, request.Points);
        await _context.Tables.AddAsync(table,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new Users(request.UserName, request.TableId, request.UserMode);
        await _context.Users.AddAsync(user,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetTables([FromQuery]GetTablesRequest request, CancellationToken ct)
    {
        var tableQuery = _context.Tables
            .Where(n => !string.IsNullOrWhiteSpace(request.Search) &&
                        n.nameTable.ToLower().Contains(request.Search.ToLower()));

        Expression<Func<Tables, object>> selectorKey = request.SortItem?.ToLower() switch
        {
            "nameTable" => tables => tables.nameTable,
            "points" => tables => tables.points,
            _ => tables => tables.id
        };

        tableQuery = request.SortOrder.Equals("desc")
            ? tableQuery.OrderByDescending(selectorKey)
            : tableQuery.OrderBy(selectorKey);

        var tableDtos = await tableQuery
            .Select(n => new TableDTO(n.id, n.nameTable, n.points))
            .ToListAsync(cancellationToken: ct);
        
        return Ok(new GetTablesResponse(tableDtos));
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery]GetTablesRequest request, CancellationToken ct)
    {
        var userQuery = _context.Users
            .Where(n => !string.IsNullOrWhiteSpace(request.Search) &&
                        n.userName.ToLower().Contains(request.Search.ToLower()));

        Expression<Func<Users, object>> selectorKey = request.SortItem?.ToLower() switch
        {
            "userName" => users => users.userName,
            "userMode" => users => users.userMode,
            "tableId" => users => users.tableId,
            _ => users => users.id
        };

        userQuery = request.SortOrder.Equals("desc")
            ? userQuery.OrderByDescending(selectorKey)
            : userQuery.OrderBy(selectorKey);

        var userDtos = await userQuery
            .Select(n => new UserDTO(n.id, n.userName, n.tableId, n.userMode))
            .ToListAsync(cancellationToken: ct);
        
        return Ok(new GetUsersResponse(userDtos));
    }
}