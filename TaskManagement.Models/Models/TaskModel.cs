using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Data.Constant;
using TaskManagement.Data.Entities;

namespace TaskManagement.Models.Models
{
    public class TaskModel
    {
        public TaskModel()
        {
            ParentId = null;
        }
        
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Performers { get; set; }

        public string RegTime { get; set; }

        [Required]
        public TreeTaskStatus Status { get; set; }

        [Required]
        public int PredictRunTime { get; set; }

        public int CurRunTime { get; set; }

        public string CompletionTime { get; set; }

        public int SubTasksPredictTime { get; set; }
        
        public int SubTasksCurTime { get; set; }

        public Guid? ParentId { get; set; }

        public TaskEntity Parent { get; set; }

        public IList<TaskEntity> Children { get; set; }
    }
}