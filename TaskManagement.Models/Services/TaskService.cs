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
    public class TaskService : ITaskDao
    {
        private readonly IDbRepository _service;
        private readonly IMapper _mapper;

        public TaskService(IDbRepository service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<TaskModel> Get(Guid id)
        {
            var entity = await _service.Get<TaskEntity>(t => t.Id == id).FirstOrDefaultAsync();

            var model = _mapper.Map<TaskModel>(entity);

            return model;
        }

        public async Task<TaskModel> Create(TaskModel taskModel)
        {
            var entity = _mapper.Map<TaskEntity>(taskModel);

            if (entity.ParentId == null)
            {
                await _service.Add(entity);
            }
            else
            {
                var parent = await _service.Get<TaskEntity>(t => t.Id == entity.ParentId).FirstOrDefaultAsync();
                parent.Children.Add(entity);
            }
            
            await _service.Save();

            return _mapper.Map<TaskModel>(entity);
        }

        public async Task<TaskModel> Update(TaskModel taskModel)
        {
            var entity = _mapper.Map<TaskEntity>(taskModel);

            var r = await _service.Update(entity);
            await _service.Save();

            return _mapper.Map<TaskModel>(r);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _service.Get<TaskEntity>(t => t.Id == id).Include(t => t.Children).FirstOrDefaultAsync();

            if (entity.Children.Count > 0)
                return false;
            
            await _service.Delete<TaskEntity>(id);
            await _service.Save();
            return true;
        }
    }
}