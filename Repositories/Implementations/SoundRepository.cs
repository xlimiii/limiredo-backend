using AutoMapper;
using limiredo_backend.Db;
using limiredo_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace limiredo_backend.Repositories.Implementations
{
    public class SoundRepository : ISoundRepository
    {
        private readonly LimiredoDbContext dbContext;
        private readonly ILogger<SoundRepository> logger;
        private readonly IMapper mapper;


        public SoundRepository(LimiredoDbContext dbContext, ILogger<SoundRepository> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            //SeedData();
        }

        public async Task<(bool IsSuccess, Models.Sound Sound, string ErrorMessage)> GetSoundAsync(int soundId)
        {
            try
            {
                logger?.LogInformation("Querying sounds");
                var sound = await dbContext.Sounds.FirstOrDefaultAsync(x => x.Id == soundId);
                if (sound != null)
                {
                    logger?.LogInformation("Sound is found");
                    var result = mapper.Map<Db.Sound, Models.Sound>(sound);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (System.Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<Models.Sound> Sounds, string ErrorMessage)> GetRandomSoundsAsync(int count = 2)
        {
            try
            {
                logger?.LogInformation("Querying sounds");
                var soundsCount = dbContext.Sounds.Count();
                var sounds = new List<Models.Sound>();
                for (int i = 0; i < count; i++)
                {
                    var random = new Random();
                    int index = random.Next(soundsCount);
                    var sound = await dbContext.Sounds.Skip(index).Take(1).FirstOrDefaultAsync();
                    sounds.Add(mapper.Map<Db.Sound, Models.Sound>(sound));
                }
                return (true, sounds, null);
            }
            catch (System.Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<Models.Sound> Sounds, string ErrorMessage)> GetRandomIntervalFromListAsync(string[] intervals, int type)
        {
            (bool IsSuccess, List<Models.Sound> Sounds, string ErrorMessage) interval;
            do
            {
                interval = await GetRandomSoundsAsync(2);
            }
            while (!intervals.Any(x => Math.Abs(interval.Sounds[1].Height - interval.Sounds[0].Height).ToString() == x));

            switch (type)
            {
                case 2:
                    interval.Sounds = interval.Sounds.OrderBy(x => x.Height).ToList();
                    break;
                case 1:
                    interval.Sounds = interval.Sounds.OrderByDescending(x => x.Height).ToList();
                    break;
                default:
                    break;
            }
            return interval;

        }
    }
}
