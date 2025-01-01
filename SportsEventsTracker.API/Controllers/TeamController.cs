using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsEventsTracker.DTO;
using SportsEventTracker.API.Data;
using SportsEventTracker.Models;

namespace SportsEventTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly SportsEventTrackerContext _context;

        public TeamController(SportsEventTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all teams.
        /// </summary>
        /// <returns>A list of teams.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Team>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams.ToListAsync();

            if (!teams.Any())
            {
                return NotFound("No teams found.");
            }

            return Ok(teams);
        }

public async Task<IActionResult> CreateTeam([FromBody] TeamDto teamDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var existingTeam = await _context.Teams.FindAsync(teamDto.TeamName);
    if (existingTeam != null)
    {
        return BadRequest($"A team with the name '{teamDto.TeamName}' already exists.");
    }

    var team = new Team
    {
        TeamName = teamDto.TeamName
    };

    _context.Teams.Add(team);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetTeams), new { teamName = team.TeamName }, team);
}

    }
}
