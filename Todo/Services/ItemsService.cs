using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todo.Controllers.Api;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models;
using Todo.Models.TodoItems;
using Todo.Services.Abstractions;

namespace Todo.Services
{
    public class ItemsService : IItemsService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<ItemsService> logger;

        public ItemsService(ApplicationDbContext context, ILogger<ItemsService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        
        public async Task<IApplicationResult<TodoItemSummaryViewmodel>> UpdateRankAsync(int id, Rank rank, CancellationToken cancellationToken)
        {
            var toDoItemEntity = await this.context.TodoItems
                .SingleOrDefaultAsync(t => t.TodoItemId == id, cancellationToken);

            if (toDoItemEntity is null)
                return ApplicationResult<TodoItemSummaryViewmodel>.FromError(ApplicationErrorMessages.NotFound,
                    HttpStatusCode.NotFound);

            var oldValue = toDoItemEntity.Rank;
            toDoItemEntity.Rank = rank;

            
            var success = await this.context.SaveChangesAsync(cancellationToken) > 0;


            if (!success)
                return ApplicationResult<TodoItemSummaryViewmodel>
                    .FromError("SOME_SMART_EXCEPTION", HttpStatusCode.InternalServerError);

            
            logger.LogInformation(
                "To do item's (id: {id}) rank updated. " + "Old value: {oldValue} New value: {newValue}", 
                id, oldValue.ToString(), rank.ToString());
            
            var viewModel = TodoItemSummaryViewmodelFactory.Create(toDoItemEntity);

            return ApplicationResult<TodoItemSummaryViewmodel>.Success(viewModel);
        }
    }
}