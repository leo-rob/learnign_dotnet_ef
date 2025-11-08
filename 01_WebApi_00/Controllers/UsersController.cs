using Microsoft.AspNetCore.Mvc;
using Core.Dtos;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(Services.IUserService userService) : ControllerBase
{
	private readonly Services.IUserService _userService = userService;
	
	
	[HttpGet]
	public Task<UserResponseDto[]> GetUsers(
		[FromQuery] bool includeRoles = false,
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		return _userService.GetUsers(includeRoles, includeProjects, includeTasks);
	}
	
	[HttpGet("by-ids")]
	public async Task<ActionResult<UserResponseDto[]>> GetUsersByIds(
		[FromQuery] int[] ids,
		[FromQuery] bool includeRoles = false,
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		return await _userService.GetUsersByIds(ids, includeRoles, includeProjects, includeTasks);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetUser(int id,
		[FromQuery] bool includeRoles = false,
		[FromQuery] bool includeProjects = false,
		[FromQuery] bool includeTasks = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var userDto = await _userService.GetUserById(id, includeRoles, includeProjects, includeTasks);
		return userDto != null ? Ok(userDto) : NotFound();
	}
	
	[HttpPost]
	public async Task<IActionResult> UpsertUser([FromBody] UserRequestDto reqUser)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var (isNewUser, user) = await _userService.UpsertUser(reqUser);
		
		if (user == null)
		{
			return NotFound("Trying to update a user that does not exist.");
		}
		
		return isNewUser ? CreatedAtAction(nameof(GetUser), new { id = user.Id }, user) : Ok(user);
	}
	
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		bool deleted =  await _userService.DeleteUser(id);
		return deleted ? NoContent() : NotFound();
	}
}