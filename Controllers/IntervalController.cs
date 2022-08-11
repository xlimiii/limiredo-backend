using limiredo_backend.Repositories.Interfaces;
using limiredo_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace limiredo_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntervalController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ISoundRepository soundRepository;
        private readonly ILogger<IntervalController> logger;

        public IntervalController(ISoundRepository soundRepository, ILogger<IntervalController> logger, IConfiguration configuration)
        {
            this.soundRepository = soundRepository;
            this.logger = logger;
            this.Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GeInterval(int type = 0, string choice = "0;1;2;3;4;5;6;7;8;9;10;11;12")
        {
            var randomSounds = await soundRepository.GetRandomIntervalFromListAsync(choice.Split(';'), type);
            var interval = Math.Abs(randomSounds.Sounds[0].Height - randomSounds.Sounds[1].Height);
            if (randomSounds.IsSuccess == true)
            {
                var intervalFile = await SoundService.GetIntervalFile(Configuration["AzureData:Url"], Configuration["AzureData:SAS"], randomSounds.Sounds);
                return File(intervalFile, "audio/wav", $"{interval}.wav");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
