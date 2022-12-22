using System.Threading;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Models;
using Todo.Models.TodoItems;

namespace Todo.Services.Abstractions
{
    public interface IItemsService
    {
        Task<IApplicationResult<TodoItemSummaryViewmodel>> UpdateRankAsync(int id, Rank rank,
            CancellationToken cancellationToken);
    }
}