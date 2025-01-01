using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsEventsTracker.DTO;
using SportsEventTracker.API.Data;
using SportsEventTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsEventTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly SportsEventTrackerContext _context;

        public MatchesController(SportsEventTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all matches with their associated teams and scores.
        /// </summary>
        /// <returns>A list of matches.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatches()
        {
            var matches = await _context.Matches
                          .Select(m=> new MatchResponseDto
                          {
                            MatchID = m.MatchID,
                            TeamAName = m.TeamAName,
                            TeamBName = m.TeamBName,
                            ScoreA = m.ScoreA,
                            ScoreB = m.ScoreB

                          })
                          .ToListAsync();
           

            if (!matches.Any())
            {
                return NotFound("No matches found.");
            }

            return Ok(matches);
        }

        /// <summary>
        /// Creates a new match.
        /// </summary>
        /// <param name="match">The match to create.</param>
        /// <returns>The created match.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GameMatch))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMatch([FromBody] GameMatchDto matchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure teams exist in the database
            var teamAExists = await _context.Teams.AnyAsync(t => t.TeamName == matchDto.TeamAName);
            var teamBExists = await _context.Teams.AnyAsync(t => t.TeamName == matchDto.TeamBName);

            if (!teamAExists || !teamBExists)
            {
                return BadRequest("Both teams must exist to create a match.");
            }
            
            var match = new GameMatch
            {
                MatchID = Guid.NewGuid(),
                TeamAName = matchDto.TeamAName,
                TeamBName = matchDto.TeamBName,
                ScoreA = matchDto.ScoreA,
                ScoreB = matchDto.ScoreB 
            };

            // Add the match to the database
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatches), new { id = match.MatchID }, match);
        }
    }
}
