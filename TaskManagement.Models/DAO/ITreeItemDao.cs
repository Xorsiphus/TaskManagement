using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.DAO
{
    public interface ITreeItemDao
    {
        Task<List<TreeItemModel>> GetRoot();

        Task<List<TreeItemModel>> GetChildren(Guid id);
    }
}