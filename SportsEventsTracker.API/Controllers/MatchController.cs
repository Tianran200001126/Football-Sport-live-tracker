using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsEventsTracker.API.Services;
using SportsEventsTracker.DTO;
using SportsEventTracker.API.Data;
using SportsEventTracker.Models;


namespace SportsEventTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly SportsEventTrackerContext _context;

        private readonly KafkaProducer<UpdateScoreDto> _kafkaProducer;

        public MatchesController(SportsEventTrackerContext context, KafkaProducer<UpdateScoreDto> kafkaProducer)
        {
            _context = context;
            _kafkaProducer = kafkaProducer;
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

        [HttpPut("update-score")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UpdateScoreDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateScore([FromBody] UpdateScoreDto updateScoreDto)
        {
            if (updateScoreDto?.TeamName == null || updateScoreDto.NewScore < 0)
            {
                return BadRequest("Invalid input data.");
            }

            var match = await _context.Matches.FirstOrDefaultAsync(m => m.MatchID == updateScoreDto.MatchID)
                        ?? throw new KeyNotFoundException($"Match with ID {updateScoreDto.MatchID} not found.");

            if (match.TeamAName == updateScoreDto.TeamName)
            {
                match.ScoreA = updateScoreDto.NewScore;
            }
            else if (match.TeamBName == updateScoreDto.TeamName)
            {
                match.ScoreB = updateScoreDto.NewScore;
            }
            else
            {
                return NotFound($"Team '{updateScoreDto.TeamName}' is not part of the match.");
            }

            await _context.SaveChangesAsync();

      

            await _kafkaProducer.ProduceAsync("update-score",updateScoreDto.MatchID.ToString(),updateScoreDto);

            return Ok(updateScoreDto);
        }
    
    }
}
