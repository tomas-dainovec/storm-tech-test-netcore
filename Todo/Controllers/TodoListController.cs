using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;

        public TodoListController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        public IActionResult Detail(int todoListId, bool? hideCompletedItems = null, string orderBy = null)
        {
            var viewmodel = CreateDetailsViewModel(todoListId, hideCompletedItems, orderBy);
            return View(viewmodel);
        }

        public IActionResult RenderListItems(int todoListId, bool? hideCompletedItems = null, string orderBy = null)
        {
            var viewmodel = CreateDetailsViewModel(todoListId, hideCompletedItems, orderBy);
            return PartialView("_ListItemsPartial", viewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new {todoList.TodoListId});
        }

        private TodoListDetailViewmodel CreateDetailsViewModel(int todoListId, bool? hideCompletedItems = null, string orderBy = null)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            return TodoListDetailViewmodelFactory.Create(todoList, hideCompletedItems, orderBy);
        }
    }
}