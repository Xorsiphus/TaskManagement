using System;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.DAO
{
    public interface ITreeTaskDao
    {
        TreeTaskModel Get(Guid id);
        Task<TreeTaskModel> Create(TreeTask task);
        Task<TreeTaskModel> Update(TreeTask task);
        Task<bool> Delete(Guid id);
    }
}