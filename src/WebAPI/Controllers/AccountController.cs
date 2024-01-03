using Application.Dto.Accounts;
using Application.Dto.Transactions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly TransactionService _transactionService;

    public AccountController(AccountService accountService, TransactionService transactionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _accountService.Get());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _accountService.Get(id));
    }

    [HttpPost]
    public async Task<IActionResult> Add(AccountAdd account)
    {
        Guid guid = await _accountService.Add(account);
        return CreatedAtAction(nameof(Get), new { Id = guid }, account);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateType(Guid id, AccountTypeEdit account)
    {
        await _accountService.UpdateType(id, account);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _accountService.Delete(id);
        return Ok();
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetAccountsTransactions(Guid id)
    {
        return Ok(await _transactionService.GetAccountsTransactions(id));
    }

    [HttpPost("{id}/topup")]
    public async Task<IActionResult> TopUp(Guid id, TopUp transaction)
    {
        await _transactionService.TopUpTransaction(id, transaction);
        return Ok();
    }

    [HttpPost("{id}/send")]
    public async Task<IActionResult> TopUp(Guid id, TransactionSend transaction)
    {
        await _transactionService.CreateTransaction(id, transaction);
        return Ok();
    }
}