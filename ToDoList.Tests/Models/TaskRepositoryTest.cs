using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;

namespace ToDoList.Tests.Models
{
    /// <summary>
    /// I wrote the unit tests on TaskRepository class rather than on the ToDoListController class because the
    /// business logic (in this case CRUD) is being implemented in the TaskRepository class.  The ToDoListController
    /// class's API (Index, Create, Details, Edit, and Delete methods) simply call the methods exposed by the 
    /// TaskRepository class.
    /// </summary>
    [TestClass]
    public class TaskRepositoryTest
    {
        public TaskRepositoryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetAllTasks_ReturnsCompleteListOfTasks()
        {
            // Arrange
            var target = new TaskRepositoryChild();
            // Act
            var taskList = target.GetAllTasks();
            // Asserts
            Assert.AreEqual(3, taskList.Count);
            Assert.AreEqual("Task1", taskList[0].Name);
            Assert.AreEqual("Task1 Description", taskList[0].Description);
            Assert.AreEqual("Task1 Details", taskList[0].Details);
            Assert.IsTrue(DateTime.Compare(new DateTime(2014, 12, 20), taskList[0].Deadline) == 0);
            Assert.IsFalse(taskList[0].Completed);
            Assert.AreEqual("Task2", taskList[1].Name);
            Assert.AreEqual("Task2 Description", taskList[1].Description);
            Assert.AreEqual("Task2 Details", taskList[1].Details);
            Assert.IsTrue(DateTime.Compare(new DateTime(2014, 12, 12), taskList[1].Deadline) == 0);
            Assert.IsTrue(taskList[1].Completed);
            Assert.AreEqual("Task3", taskList[2].Name);
            Assert.AreEqual("Task3 Description", taskList[2].Description);
            Assert.AreEqual("Task3 Details", taskList[2].Details);
            Assert.IsTrue(DateTime.Compare(new DateTime(2014, 12, 24), taskList[2].Deadline) == 0);
            Assert.IsFalse(taskList[2].Completed);
        }

        [TestMethod]
        public void GetTaskByTaskId_ReturnsOnlyTaskOfTheGivenTaskId()
        {
            // Arrange
            var target = new TaskRepositoryChild();
            // Act
            var task = target.GetTask(2);
            // Assert
            Assert.AreEqual("Task2", task.Name);
            Assert.AreEqual("Task2 Description", task.Description);
            Assert.AreEqual("Task2 Details", task.Details);
            Assert.IsTrue(DateTime.Compare(new DateTime(2014, 12, 12), task.Deadline) == 0);
            Assert.IsTrue(task.Completed);
        }

        [TestMethod]
        public void AddTask_ReturnsAddedTaskWithNewTaskId()
        {
            // Arrange
            var taskToAdd = new Task()
            {
                Name = "Task4",
                Description = "Task4 Description",
                Details = "Task4 Details",
                Deadline = new DateTime(2014, 12, 31),
                Completed = false
            };
            
            var target = new TaskRepositoryChild();

            // Act
            var task = target.AddTask(taskToAdd);

            // Assert
            Assert.AreEqual(4, task.Id);
            Assert.AreEqual("Task4", task.Name);
            var taskList = target.GetAllTasks();
            Assert.AreEqual(4, taskList.Count);
        }

        [TestMethod]
        public void UpdateTask_ReturnsFalseIfTaskIdNotFound()
        {
            // Arrange
            var taskWithUpdateValue = new Task()
            {
                Name = "Task3",
                Description = "Task3 New Description",
                Details = "Task4 New Details",
                Deadline = new DateTime(2014, 12, 31),
                Completed = true
            };

            var taskId = 10;
            var target = new TaskRepositoryChild();

            // Act
            var result = target.UpdateTask(taskId, taskWithUpdateValue);

            // Asserts
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTask_ReturnsTrueIfTaskIdFoundAndVerifyTaskHasUpdatedValue()
        {
            // Arrange
            var taskWithUpdateValue = new Task()
            {
                Name = "Task3-1",
                Description = "Task3 New Description",
                Details = "Task3 New Details",
                Deadline = new DateTime(2014, 12, 31),
                Completed = true
            };

            var taskId = 3;
            var target = new TaskRepositoryChild();

            // Act
            var result = target.UpdateTask(taskId, taskWithUpdateValue);
            Task updatedTask = null;
            if (result)
            {
                updatedTask = target.GetTask(taskId);
            }
            
            // Asserts
            Assert.IsTrue(result);
            Assert.AreEqual("Task3-1", updatedTask.Name);
            Assert.AreEqual("Task3 New Description", updatedTask.Description);
            Assert.AreEqual("Task3 New Details", updatedTask.Details);
            Assert.IsTrue(DateTime.Compare(new DateTime(2014, 12, 31), updatedTask.Deadline) == 0);
            Assert.IsTrue(updatedTask.Completed);
        }

        [TestMethod]
        public void RemoveTask_VerifyListCountIsDecrementedByOneAndCorrectTaskIsRemovedFromList()
        {
            // Arrange
            var taskId = 2;
            var target = new TaskRepositoryChild();
            var taskCountBeforeRemove = target.GetAllTasks().Count;

            // Act
            target.RemoveTask(taskId);
            var taskList = target.GetAllTasks();

            // Assert
            Assert.AreEqual(taskCountBeforeRemove - 1, taskList.Count);
            Assert.IsFalse(taskList.Exists(t => t.Id == taskId));
        }
    }

    /// <summary>
    /// Create the child class of TaskRepository so I can override the PopulateInitialTodoList().
    /// This allows the unit test project to test with its own mocked data.
    /// </summary>
    public class TaskRepositoryChild : TaskRepository
    {
        public TaskRepositoryChild():base()
        {
        }

        protected override void PopulateInitialTodoList()
        {
            _todoTasks.Add(new Task
            {
                Id = 1,
                Name = "Task1",
                Description = "Task1 Description",
                Details = "Task1 Details",
                Deadline = new DateTime(2014, 12, 20),
                Completed = false
            });
            _nextId++;
            _todoTasks.Add(new Task
            {
                Id = 2,
                Name = "Task2",
                Description = "Task2 Description",
                Details = "Task2 Details",
                Deadline = new DateTime(2014, 12, 12),
                Completed = true
            });
            _nextId++;
            _todoTasks.Add(new Task
            {
                Id = 3,
                Name = "Task3",
                Description = "Task3 Description",
                Details = "Task3 Details",
                Deadline = new DateTime(2014, 12, 24),
                Completed = false
            });
            _nextId++;
        }
    }
}
