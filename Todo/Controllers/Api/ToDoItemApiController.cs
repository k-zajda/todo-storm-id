using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Data.Entities;
using Todo.Services.Abstractions;

namespace Todo.Controllers.Api
{
    [Route("api/v1/to-do-items")]
    public class ToDoItemApiController : ControllerBase
    {
        private readonly IItemsService itemsService;

        [HttpPut("/{id}/rank/{value}")]
        public async Task<IActionResult> UpdateRankAsync([FromRoute] int id, [FromRoute] Rank rank,
            CancellationToken cancellationToken)
        {
            var result = await this.itemsService.UpdateRankAsync(id, rank, cancellationToken);
            
            if (result.IsSuccess)
                return Ok(result.Resource);

            return StatusCode((int) result.StatusCode, result.ErrorMessages);
        }
    }
}
