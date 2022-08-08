using limiredo_backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace limiredo_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundController : ControllerBase
    {
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<SoundController> logger;

        public SoundController(ISoundRepository soundRepository, ILogger<SoundController> logger)
        {
            this.soundRepository = soundRepository;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSoundAsync(int id)
        {
            var result = await soundRepository.GetSoundAsync(id);
            if (result.IsSuccess == true)
            {
                return Ok(result.Sound);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomSounds(int count=2)
        {
            var result = await soundRepository.GetRandomSoundsAsync(count);
            if (result.IsSuccess == true)
            {
                return Ok(result.Sounds);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
