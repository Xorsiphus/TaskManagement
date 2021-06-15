using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Data.Entities;
using TaskManagement.Models.DAO;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.Services
{
    public class TreeItemService : ITreeItemDao
    {
        private readonly IDbRepository _service;
        private readonly IMapper _mapper;

        public TreeItemService(IDbRepository repository, IMapper mapper)
        {
            _service = repository;
            _mapper = mapper;
        }

        public async Task<List<TreeItemModel>> GetRoot()
        {
            var entities = await _service.Get<TaskEntity>(t => t.ParentId == null).Include(t => t.Children).ToListAsync();

            if (entities == null)
                return null;
            
            var items = _mapper.Map<List<TreeItemModel>>(entities);
            return items;
        }

        public async Task<List<TreeItemModel>> GetChildren(Guid id)
        {
            var entities = await _service.Get<TaskEntity>(t => t.ParentId == id).Include(t => t.Children).ToListAsync();
           
            if (entities == null)
                return null;
            
            var items = _mapper.Map<List<TreeItemModel>>(entities);
            return items;
        }
    }
}