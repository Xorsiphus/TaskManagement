using TaskManagement.Data.Entities;
using TaskManagement.Models.Models;

namespace TaskManagement.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<TreeTask, TreeTaskModel>();
            CreateMap<TreeTaskModel, TreeTask>();
        }
    }
}