using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    /// <summary>
    /// This is the controller class that controls the TODO task and task list to display to
    /// the View, as well as creating a new task, updating an existing task, and deleting a
    /// task as requested by the user from the View.
    /// 
    /// NOTE:
    /// In this design, the controller is injected with an instance of the TaskRepository class
    /// through its constructor (i.e. dependency injection).  It then uses the TaskRepository
    /// instance to execute all the operations it needs to do.  For this reason, I have written
    /// my unit tests against the TaskRepository class rather than this class.
    /// </summary>
    public class ToDoListController : Controller
    {
        private ITaskRepository _taskRepository;

        /// <summary>
        /// taskRepository is an instance of the TaskRepository class which was created by the
        /// Unity Framework.  Check out the UnityConfig.cs in the App_Start folder to see where
        /// the class is registerd to the ITaskRepositoy interface.
        /// </summary>
        /// <param name="taskRepository"></param>
        public ToDoListController(ITaskRepository taskRepository)
        {
            if (taskRepository == null)
            {
                throw new ArgumentNullException("taskRepository");
            }

            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Control what to send to the Index page for the list of the TODO tasks
        /// It gets all the TODO tasks in the list by calling the TaskRepository's public method (i.e. API)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_taskRepository.GetAllTasks());
        }

        /// <summary>
        /// Get the details of the task with the given task ID and present it to the Details page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View(_taskRepository.GetTask(id));
        }

        /// <summary>
        /// This method is executed when user clicks on the Create link on the Index page.
        /// It returns the view of the Create page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method is executed when user clicks the Submit button on the Create page after  
        /// he finishes populating the Create page with the new task data.  It assigs a new task  
        /// ID and adds the new task to the List.  When completed, it redirects to the Index page.
        /// Otherwise, it stays on the Create page.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Task task)
        {
            try
            {
                _taskRepository.AddTask(task);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// This method is executed when user clicks on the Edit link of a particular task.
        /// It gets the task from list and returns the Edit page (or view).
        /// 
        /// NOTE:  
        /// One can check whether the task being returned by GetTask(id) is null.  However,
        /// such check is redundant here as the ID is never visible much less modifiable by
        /// user, and we only show what is available in the TODO list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var task = _taskRepository.GetTask(id);
            return View(task);
        }

        /// <summary>
        /// This method is executed when user clicks on the Submit button on the Edit page.
        /// It updates the content of the task indexed by the task ID.  If the update is
        /// successful, it redirects to the Index page.  Otherwise, it stays on the Edit page.
        /// 
        /// NOTE:
        /// The UpdateTask() returns false if the task ID has no match in the list. However, 
        /// in this implementation this is not possible since we show only the tasks in the list
        /// and user has no access or visibility to the task ID.  So it might be ok to make 
        /// UpdateTask() to return void.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Task task)
        {
            if (_taskRepository.UpdateTask(task.Id, task) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// This method is excuted when user clicks the Delete link to remove the task in the
        /// same row.  Once deleted, the method redirect back to the Index page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            _taskRepository.RemoveTask(id);
            return RedirectToAction("Index");
        }
	}
}