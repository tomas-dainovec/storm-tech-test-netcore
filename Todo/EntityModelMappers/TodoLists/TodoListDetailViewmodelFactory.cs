using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool? hideCompletedItems, string orderBy)
        {
            var filteredItems = todoList.Items.AsEnumerable();
            if (hideCompletedItems == true)
            {
                filteredItems = filteredItems.Where(i => !i.IsDone);
            }

            if (orderBy == nameof(TodoItem.Rank))
            {
                filteredItems = filteredItems.OrderBy(i => i.Rank ?? int.MaxValue);
            }
            else
            {
                filteredItems = filteredItems.OrderBy(i => i.Importance);
            }
            
            var items = filteredItems.Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, hideCompletedItems, orderBy);
        }
    }
}