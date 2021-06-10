using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskManagement.Data.Constant;

namespace TaskManagement.Models.Models
{
    public class TaskModel
    {
        public TaskModel()
        {
            ParentId = null;
        }
        
        [BindNever]
        public Guid Id { get; set; }

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

        [Required]
        public int CurRunTime { get; set; }
        
        public string CompletionTime { get; set; }

        public int SubTasksPredictTime { get; set; }
        
        public int SubTasksCurTime { get; set; }

        public Guid? ParentId { get; set; }
        //
        // public IList<TaskModel> Children { get; set; }
    }
}