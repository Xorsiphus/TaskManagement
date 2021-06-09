using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Data.Constant;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data
{
    public static class DbContentInit
    {
        public static void Initial(AppDbContext context)
        {
            if (!context.TreeTasks.Any())
            {
                var tasks = new List<TaskEntity>
                {
                    new TaskEntity
                    {
                        Name = "First",
                        Description = "Description",
                        Performers = "performers",
                        Status = TreeTaskStatus.Appointed,
                        PredictRunTime = 123,
                        CurRunTime = 10,
                    },
                    new TaskEntity
                    {
                        Name = "Second",
                        Description = "Description 2",
                        Performers = "performers 2",
                        Status = TreeTaskStatus.Paused,
                        PredictRunTime = 312,
                        CurRunTime = 33,
                    }
                };

                tasks.ForEach(t => context.TreeTasks.Add(t));
                context.TreeTasks.Add(new TaskEntity
                {
                    Name = "Third",
                    Description = "Description 3",
                    Performers = "performers 3",
                    Status = TreeTaskStatus.Completed,
                    PredictRunTime = 55,
                    CurRunTime = 11,
                    ParentId = tasks.Find(g => g.Name == "First")?.Id
                });
                
                context.SaveChanges();
            }
        }
    }
}