using Microsoft.AspNetCore.Mvc;
using ToDo_Server.Entities;
using ToDo_Server.Service;

namespace ToDo_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly MongoDbService _mongoDBService;

        public ToDoController(MongoDbService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoItem>>> Get()
        {
            return await _mongoDBService.GetAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> Create(ToDoItem toDo)
        {
            await _mongoDBService.CreateItem(toDo);
            return CreatedAtAction("Get", new { id = toDo.Id.ToString() }, toDo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ToDoItem toDoIn)
        {
            await _mongoDBService.UpdateAsync(id, toDoIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mongoDBService.RemoveAsync(id);

            return NoContent();
        }
    }
}
