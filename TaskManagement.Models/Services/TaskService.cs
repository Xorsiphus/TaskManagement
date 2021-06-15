using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Data.Constant;
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
            var entity = await _service
                .Get<TaskEntity>(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            _service.LoadChildrenRecursively(entity);

            var model = _mapper.Map<TaskModel>(entity);

            model.SubTasksPredictTime = CalculateSubPredicted(model) - model.PredictRunTime;
            model.SubTasksCurTime = CalculateSubCurrent(model) - model.CurRunTime;
            model.Children = null;

            return model;
        }

        public async Task<TaskModel> Create(TaskModel taskModel)
        {
            var entity = _mapper.Map<TaskEntity>(taskModel);

            if (entity.ParentId == null)
            {
                if (await _service.Add(entity) == null)
                    return null;
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
            var entity = await _service.Get<TaskEntity>(t => t.Id == taskModel.Id).FirstOrDefaultAsync();

            return (entity.Status, taskModel.Status) switch
            {
                (TreeTaskStatus.Appointed, TreeTaskStatus.Appointed) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.InProgress, TreeTaskStatus.InProgress) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.Paused, TreeTaskStatus.Paused) => await DoUpdate(entity, taskModel),
                // (TreeTaskStatus.Completed, TreeTaskStatus.Completed) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.Appointed, TreeTaskStatus.InProgress) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.InProgress, TreeTaskStatus.Paused) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.Paused, TreeTaskStatus.InProgress) => await DoUpdate(entity, taskModel),
                (TreeTaskStatus.InProgress, TreeTaskStatus.Completed) => await DoUpdate(entity, taskModel),
                _ => null
            };
        }

        private async Task<TaskModel> DoUpdate(TaskEntity entity, TaskModel model)
        {
            _mapper.Map(model, entity);
            
            _service.LoadChildrenRecursively(entity);
            
            if (model.Status == TreeTaskStatus.Completed && TryCompleteTask(entity))
            {
                entity.CompletionTime = DateTime.Now;
                entity.CurRunTime = Convert.ToInt32((entity.CompletionTime - entity.RegTime).TotalHours);
                
                model.CompletionTime = DateTime.Now.ToString("G");
                model.CurRunTime = entity.CurRunTime;
                
                DoCompleteTask(entity);
            }

            await _service.Save();

            return model;
        }
        
        private static bool TryCompleteTask(TaskEntity entity)
        {
            foreach (var child in entity.Children)
            {
                if (child.Status == TreeTaskStatus.Appointed || 
                    child.Status == TreeTaskStatus.Paused)
                    return false;
                if (!TryCompleteTask(child))
                    return false;
            }
            return true;
        }

        private static void DoCompleteTask(TaskEntity entity)
        {
            foreach (var child in entity.Children)
            {
                child.Status = TreeTaskStatus.Completed;
                DoCompleteTask(child);
            }
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

        private static int CalculateSubPredicted(TaskModel model)
        {
            return model.PredictRunTime + model.Children.Sum(m => CalculateSubPredicted(m));
        }

        private static int CalculateSubCurrent(TaskModel model)
        {
            return model.CurRunTime + model.Children.Sum(CalculateSubCurrent);
        }
    }
}