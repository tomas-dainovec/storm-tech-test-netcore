using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool hideCompletedItems)
        {
            var filteredItems = todoList.Items.AsEnumerable();
            if (hideCompletedItems)
            {
                filteredItems = filteredItems.Where(i => !i.IsDone);
            }
            
            var items = filteredItems.Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, hideCompletedItems);
        }
    }
}