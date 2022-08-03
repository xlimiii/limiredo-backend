using AutoMapper;

namespace limiredo_backend.Profiles
{
    public class SoundProfile : Profile
    {
        public SoundProfile()
        {
            CreateMap<Db.Sound, Models.Sound>();
        }
    }
}
