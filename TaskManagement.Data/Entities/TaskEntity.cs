using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Data.Constant;

namespace TaskManagement.Data.Entities
{
    public class TaskEntity : IEntity
    {
        public TaskEntity()
        {
            Children = new List<TaskEntity>();
        }
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Performers { get; set; }
        
        public DateTime RegTime { get; set; } = DateTime.Now;

        public TreeTaskStatus Status { get; set; }

        public int PredictRunTime { get; set; }

        public int CurRunTime { get; set; }

        public DateTime CompletionTime { get; set; }

        [ForeignKey("TreeTasks")]
        public Guid? ParentId { get; set; }

        public TaskEntity Parent { get; set; }

        public IList<TaskEntity> Children { get; set; }
    }
}