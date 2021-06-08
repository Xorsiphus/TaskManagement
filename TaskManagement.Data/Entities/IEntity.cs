using System;

namespace TaskManagement.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}