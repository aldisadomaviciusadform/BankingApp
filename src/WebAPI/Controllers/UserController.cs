using Application.Dtos.Users;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// This is a sample controller for demonstrating XML comments in ASP.NET Core.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets all items
    /// </summary>
    /// <returns>list of items</returns>
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
}