using Application.Dto.Users;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AccountService _accountService;
    private readonly TransactionService _transactionService;

    public UserController(UserService userService, AccountService accountService, TransactionService transactionService)
    {
        _userService = userService;
        _accountService = accountService;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.Get());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _userService.Get(id));
    }

    [HttpGet("{id}/accounts")]
    public async Task<IActionResult> GetUserAccounts(Guid id)
    {
        return Ok(await _accountService.GetUserAccounts(id));
    }

    [HttpPost]
    public async Task<IActionResult> Add(UserAdd item)
    {
        Guid guid = await _userService.Add(item);
        return CreatedAtAction(nameof(Get), new { Id = guid }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UserAdd _item)
    {
        await _userService.Update(id, _item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.Delete(id);
        return Ok();
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetUsersTransactions(Guid id)
    {
        return Ok(await _transactionService.GetUsersTransactions(id));
    }
}