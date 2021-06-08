using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace TaskManagement.Data.Entities
{
    public class TreeTask : IEntity
    {
        public TreeTask()
        {
            Children = new List<TreeTask>();
        }
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Performers { get; set; }
        
        public DateTime RegTime { get; set; } = DateTime.Now;

        public TaskStatus Status { get; set; }

        public int PredictRunTime { get; set; }

        public int CurRunTime { get; set; }

        public DateTime CompletionTime { get; set; }

        [ForeignKey("TreeTasks")]
        public Guid? ParentId { get; set; }

        public TreeTask Parent { get; set; }

        public IList<TreeTask> Children { get; set; }
    }
}