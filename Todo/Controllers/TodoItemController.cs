using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils.Messaging;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create(int todoListId)
        {
            var fields = CreateTodoItemCreateFieldsViewModel(todoListId);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            await PersistTodoItem(fields);

            return RedirectToListDetail(fields.TodoListId);
        }

        [HttpGet]
        public IActionResult Edit(int todoItemId)
        {
            var todoItem = dbContext.SingleTodoItem(todoItemId);
            var fields = TodoItemEditFieldsFactory.Create(todoItem);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoItemEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(todoItem.TodoListId);
        }

        [HttpGet]
        public IActionResult RenderCreateFieldsPartial(int todoListId)
        {
            var fields = CreateTodoItemCreateFieldsViewModel(todoListId);
            return PartialView("_CreateFieldsPartial", fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleCreateForm(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return PartialView("_CreateFieldsPartial", fields); }

            await PersistTodoItem(fields);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRank(int todoListId, int todoItemId, int newRank)
        {
            var items = await dbContext.TodoItems
                .Where(i => i.TodoListId == todoListId)
                .OrderBy(i => i.Rank ?? int.MaxValue)
                .ToListAsync();

            var updatedItem = items.First(i => i.TodoItemId == todoItemId);

            var updatedItemIndex = items.IndexOf(updatedItem);

            updatedItem.Rank = newRank;

            // Down
            if (newRank - 1 > updatedItemIndex)
            {
                for (var i = updatedItemIndex + 1; i <= newRank - 1; i++)
                {
                    items[i].Rank = i;
                }
            }
            // Up
            else
            {
                for (var i = newRank - 1; i < updatedItemIndex; i++)
                {
                    items[i].Rank = i + 2;
                }
            }

            dbContext.TodoItems.UpdateRange(items);

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new {todoListId = fieldsTodoListId});
        }

        private TodoItemCreateFields CreateTodoItemCreateFieldsViewModel(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            return TodoItemCreateFieldsFactory.Create(todoList, User.Id());
        }

        private async Task PersistTodoItem(TodoItemCreateFields fields)
        {
            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}