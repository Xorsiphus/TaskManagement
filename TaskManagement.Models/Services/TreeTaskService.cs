using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Data.Entities;
using TaskManagement.Models.DAO;
using TaskManagement.Models.Models;

namespace TaskManagement.Models.Services
{
    public class TreeTaskService : ITreeTaskDao
    {
        private readonly IDbRepository _service;
        private readonly IMapper _mapper;

        public TreeTaskService(IDbRepository service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public TreeTaskModel Get(Guid id)
        {
            var entity = _service.Get<TreeTask>(t => t.Id == id).FirstOrDefault();

            var model = _mapper.Map<TreeTaskModel>(entity);

            return model;
        }

        public async Task<TreeTaskModel> Create(TreeTask task)
        {
            var entity = _mapper.Map<TreeTask>(task);

            if (entity.ParentId == null)
            {
                await _service.Add(entity);
            }
            else
            {
                var parent = await _service.Get<TreeTask>(t => t.Id == entity.ParentId).FirstOrDefaultAsync();
                parent.Children.Add(entity);
            }
            
            await _service.Save();

            return _mapper.Map<TreeTaskModel>(entity);
        }

        public async Task<TreeTaskModel> Update(TreeTask task)
        {
            var entity = _mapper.Map<TreeTask>(task);

            var r = await _service.Update(entity);
            await _service.Save();

            return _mapper.Map<TreeTaskModel>(r);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _service.Get<TreeTask>(t => t.Id == id).Include(t => t.Children).FirstOrDefaultAsync();

            if (entity.Children.Count > 0)
                return false;
            
            await _service.Delete<TreeTask>(id);
            await _service.Save();
            return true;
        }
    }
}