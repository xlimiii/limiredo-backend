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

        //TODO - delete (just for developing purposes only)
        private void SeedData()
        {
            if (!dbContext.Sounds.Any())
            {
                dbContext.Sounds.Add(new Db.Sound() { Id = 1, File = "File1", Height = 15, SourceId = 0});
                dbContext.Sounds.Add(new Db.Sound() { Id = 2, File = "File2", Height = 16, SourceId = 0 });
                dbContext.Sounds.Add(new Db.Sound() { Id = 3, File = "File3", Height = 17, SourceId = 0 });

                dbContext.SaveChanges();
            }
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
    }
}
