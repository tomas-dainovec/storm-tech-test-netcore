using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool? HideCompletedItems { get; }
        public string OrderBy { get; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool? hideCompletedItems, string orderBy)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            HideCompletedItems = hideCompletedItems;
            OrderBy = orderBy;
        }
    }
}