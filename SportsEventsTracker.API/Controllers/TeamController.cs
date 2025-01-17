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


        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="teamDto">The team data transfer object containing the name of the team to be created.</param>
        /// <returns>The created team object or an error if the team already exists.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Team
        ///     {
        ///        "teamName": "Arsenal"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Returns the newly created team</response>
        /// <response code="400">If the team already exists or the model is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Team))] // 201 Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400 Bad Request
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
