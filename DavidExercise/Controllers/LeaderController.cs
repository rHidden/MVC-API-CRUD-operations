using DavidExercise.Models;
using DavidExercise.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DavidExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderController : ControllerBase
    {
        private readonly ILeaderService _leaderService;
        public LeaderController(ILeaderService leaderService)
        {
            _leaderService = leaderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaderAsync(int id)
        {
            Leader leader = await _leaderService.GetLeader(id);
            if (leader == null)
            {
                return NotFound();
            }
            return Ok(leader);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> ListLeadersAsync()
        {
            List<Leader> leaders = await _leaderService.ListLeaders();
            if (!leaders.Any())
            {
                return NotFound();
            }
            return Ok(leaders);
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateLeader(Leader leader)
        {
            var createdLeader = await _leaderService.CreateLeader(leader);
            return Ok(createdLeader);
        }

        [HttpPatch]
        public IActionResult UpdateLeader(Leader leader)
        {
            _leaderService.UpdateLeader(leader);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteLeader(int id)
        {
            _leaderService.DeleteLeader(id);
            return Ok();
        }
    }
}
