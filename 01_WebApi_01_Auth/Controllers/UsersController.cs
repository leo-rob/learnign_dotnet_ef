using Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roles;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UsersController(Services.IUserService userService) : ControllerBase
{
	private readonly Services.IUserService _userService = userService;
	
	
	[HttpGet]
	public Task<UserResponseFullDto[]> GetUsers(
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		return _userService.GetUsers(includeProjects, includeTasks);
	}
	
	[HttpGet("by-ids")]
	public async Task<ActionResult<UserResponseFullDto[]>> GetUsersByIds(
		[FromQuery] Guid[] ids,
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		return await _userService.GetUsersByIds(ids, includeProjects, includeTasks);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetUser(Guid id,
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var userDto = await _userService.GetUserById(id, includeProjects, includeTasks);
		return userDto != null ? Ok(userDto) : NotFound();
	}
	
	// This call is limited to Admin role only
	[HttpPost]
	[Authorize(Roles = AppRoles.SuperAdmin + "," + AppRoles.Admin)]
	public async Task<IActionResult> UpsertUser([FromBody] UserRequestDto reqUser)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var (isNewUser, user, code, errAdd) = await _userService.UpsertUser(HttpContext.User, reqUser);
		if (code.HasValue)
		{
			return new ObjectResult(errAdd)
			{
				StatusCode = code
			};
		}
		
		return isNewUser ? CreatedAtAction(nameof(GetUser), new { id = user!.Id }, user) : Ok(user);
	}
	
	[HttpDelete("{id}")]
	[Authorize(Roles = AppRoles.SuperAdmin + "," + AppRoles.Admin)]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		bool deleted =  await _userService.DeleteUser(id);
		return deleted ? NoContent() : NotFound();
	}
}