using BattleSimulator.Data;
using BattleSimulator.Interfaces;
using BattleSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using static BattleSimulator.Support;

namespace BattleSimulator.Controllers
{
    [Route("api/arena")]
    [ApiController]
    public class ArenaController : ControllerBase
    {
        private readonly IArenaRepository _arenaRepo;
        private readonly ISoldierRepository _soldierRepo;
        public ArenaController(IArenaRepository arenaRepo, ISoldierRepository soldierRepo)
        {
            _arenaRepo = arenaRepo;
            _soldierRepo = soldierRepo;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetAllArenas()
        {
            var Arenas = await _arenaRepo.GetAllAsync();
            return Ok(Arenas);
        }*/

        [HttpGet]
        [Route("{n}")]
        public async Task<IActionResult> GenerateArena([FromRoute] int n)
        {
            await _arenaRepo.CreateAsync();
            var Arenas = await _arenaRepo.GetAllAsync();

            if (Arenas.IsNullOrEmpty())
            {
                return BadRequest();
            }

            int id = Arenas.Last().Id;
            await _soldierRepo.CreateMultipleAsync(id, n);

            return Ok(id);
        }

        /*[HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteArena([FromRoute] int id)
        {
            await _arenaRepo.DeleteAsync(id);
            return NoContent();
        }*/

        [HttpGet("{id}/battle")]
        public async Task<IActionResult> PerformBattle([FromRoute] int id)
        {
            var arena = await _arenaRepo.DoBattleAsync(id);
            if(arena is null)
                return BadRequest();

            var eventDescriptions = arena.Events.Select(e => e.Description).ToList();

            return Ok(eventDescriptions);
        }
    }
}
