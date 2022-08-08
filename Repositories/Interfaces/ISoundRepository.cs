using limiredo_backend.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace limiredo_backend.Repositories.Interfaces
{
    public interface ISoundRepository
    {
        Task<(bool IsSuccess, Models.Sound Sound, string ErrorMessage)> GetSoundAsync(int soundId);
        Task<(bool IsSuccess, List<Models.Sound> Sounds, string ErrorMessage)> GetRandomSoundsAsync(int count);

    }
}
