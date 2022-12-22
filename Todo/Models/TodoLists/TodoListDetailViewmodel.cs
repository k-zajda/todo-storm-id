using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool OrderByRankAscending { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool orderByRankAscending = true)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            OrderByRankAscending = orderByRankAscending;
        }
    }
}