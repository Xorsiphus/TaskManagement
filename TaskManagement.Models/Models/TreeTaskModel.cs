using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Data.Entities;

namespace TaskManagement.Models.Models
{
    public class TreeTaskModel
    {
        public TreeTaskModel()
        {
            ParentId = null;
        }
        
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Performers { get; set; }

        public DateTime RegTime { get; set; } = DateTime.Now;

        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        public int PredictRunTime { get; set; }

        public int CurRunTime { get; set; }

        public DateTime CompletionTime { get; set; }

        public int SubTasksPredictTime { get; set; }
        
        public int SubTasksCurTime { get; set; }

        public Guid? ParentId { get; set; }

        public TreeTask Parent { get; set; }

        public IList<TreeTask> Children { get; set; }
    }
}