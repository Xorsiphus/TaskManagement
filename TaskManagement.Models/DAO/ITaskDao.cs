using System;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.DAO
{
    public interface ITaskDao
    {
        Task<TaskModel> Get(Guid id);
        Task<TaskModel> Create(TaskModel taskModel);
        Task<TaskModel> Update(TaskModel taskModel);
        Task<bool> Delete(Guid id);
    }
}