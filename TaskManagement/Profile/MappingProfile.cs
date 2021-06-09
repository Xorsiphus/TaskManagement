using System;
using TaskManagement.Data.Constant;
using TaskManagement.Data.Entities;
using TaskManagement.Models.Models;

namespace TaskManagement.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskEntity, TaskModel>()
                .ForMember(d => d.RegTime, opt => opt.MapFrom(e => e.RegTime.ToString("G")))
                .ForMember(d => d.CompletionTime, opt => opt.MapFrom(e => e.CompletionTime.ToString("G")));

            CreateMap<TaskModel, TaskEntity>().ForMember(d => d.CompletionTime,
                opt => opt.MapFrom(m => DateTime.Parse(m.CompletionTime)));
            
            CreateMap<TaskEntity, TreeItemModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(e => e.Id.ToString()))
                .ForMember(d => d.Parent, opt => opt.MapFrom(e => e.ParentId == null ? "" : e.ParentId.ToString()))
                .AfterMap((source, d) =>
                {
                    d.Status = source.Status switch
                    {
                        TreeTaskStatus.Appointed => "1",
                        TreeTaskStatus.InProgress => "2",
                        TreeTaskStatus.Paused => "3",
                        TreeTaskStatus.Completed => "0",
                        _ => "",
                    };
                    if (source.Children.Count > 0)
                        d.IsParent = true;
                });
        }
    }
}