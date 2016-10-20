using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    /// <summary>
    /// This class contains the business logic to do the CRUD operations on the TODO list.
    /// Normally, this class would interact with an external component (e.g. database) to get/post
    /// data.  For simplicity, we just use a static list to cache TODO tasks in memory as this is
    /// allowed by the quiz.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        protected static List<Task> _todoTasks = new List<Task>();  // This is the TODO list
        protected static bool _firstTime = true;

        // The _nextId is used to assign to the task ID as it is added to the TODO list.
        // It gets incremented by 1 each time after a new task is added to the list.
        // It is analogous to the primary key (identity seed) in the SQL database.
        protected static int _nextId = 1;

        /// <summary>
        /// The constructor populates the TODO list with the initial list of data when instantiated for
        /// the first time.  This is somewhat of an anomaly as data are usually fetched from a persistant
        /// storage (e.g. database, files).
        /// </summary>
        public TaskRepository()
        {
            if (_firstTime)
            {
                PopulateInitialTodoList();
                _firstTime = false;
            }
        }

        public List<Task> GetAllTasks()
        {
            return _todoTasks.ToList();
        }

        public Task GetTask(int id)
        {
            var task = _todoTasks.Find(t => t.Id == id);
            return task;
        }

        public Task AddTask(Task task)
        {
            task.Id = _nextId++;
            _todoTasks.Add(task);
            return task;
        }

        public bool UpdateTask(int id, Task task)
        {
            var taskToUpdate = _todoTasks.Find(t => t.Id == id);
            if (taskToUpdate == null)
            {
                return false;
            }

            taskToUpdate.Name = task.Name;
            taskToUpdate.Description = task.Description;
            taskToUpdate.Details = task.Details;
            taskToUpdate.Deadline = task.Deadline;
            taskToUpdate.Completed = task.Completed;
            return true;
        }

        public void RemoveTask(int id)
        {
            _todoTasks.RemoveAll(t => t.Id == id);
        }

        /// <summary>
        /// This method is used to initially populate the TODO list when the class is first instantiated.
        /// The method has access modifier of protected virtual so it can be overridden by its subclasses.
        /// See the unit test project (TaskRepositoryTest.cs) as it creates a subclass to override this
        /// method to populate the TODO list with its own test data.
        /// </summary>
        protected virtual void PopulateInitialTodoList()
        {
            _todoTasks.Clear();

            _todoTasks.Add(new Task
            {
                Id = 1,
                Name = "Code deployment",
                Description = "Deployment to QA",
                Details = "Must be completed by deadline!",
                Deadline = new DateTime(2014, 12, 20),
                Completed = false
            });
            _nextId++;
            _todoTasks.Add(new Task
            {
                Id = 2,
                Name = "Code Review",
                Description = "Review Tom's code",
                Details = "Make sure he included unit tests",
                Deadline = new DateTime(2014, 12, 12),
                Completed = false
            });
            _nextId++;
            _todoTasks.Add(new Task
            {
                Id = 3,
                Name = "Meeting with Jeff",
                Description = "Discuss next sprint planning",
                Details = "Address localization issue",
                Deadline = new DateTime(2014, 12, 25),
                Completed = false
            });
            _nextId++;
        }
    }
}