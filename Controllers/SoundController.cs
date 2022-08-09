using limiredo_backend.Repositories.Interfaces;
using limiredo_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace limiredo_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<SoundController> logger;

        public SoundController(ISoundRepository soundRepository, ILogger<SoundController> logger, IConfiguration configuration)
        {
            this.soundRepository = soundRepository;
            this.logger = logger;
            this.Configuration = configuration;
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
            var randomSounds = await soundRepository.GetRandomSoundsAsync(count);
           
            if (randomSounds.IsSuccess == true)
            {
                var intervalFile = await SoundService.GetIntervalFile(Configuration["AzureData:Url"], Configuration["AzureData:SAS"], randomSounds.Sounds);
                return File(intervalFile, "audio/wav", "test.wav");
            }
            else
            {
                return NotFound();
            }
        }


       

    
    }
}
