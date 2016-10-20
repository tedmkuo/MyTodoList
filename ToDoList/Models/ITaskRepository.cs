using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public interface ITaskRepository
    {
        List<Task> GetAllTasks();

        Task GetTask(int id);

        Task AddTask(Task task);

        bool UpdateTask(int id, Task task);

        void RemoveTask(int id);
    }
}
