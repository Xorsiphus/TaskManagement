using System;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.DAO
{
    public interface ITaskDao
    {
        TaskModel Get(Guid id);
        Task<TaskModel> Create(TaskEntity taskEntity);
        Task<TaskModel> Update(TaskEntity taskEntity);
        Task<bool> Delete(Guid id);
    }
}