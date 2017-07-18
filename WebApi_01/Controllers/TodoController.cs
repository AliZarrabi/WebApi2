using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_01.Models;

namespace WebApi_01.Controllers
{
    [Produces("application/json")]
    [Route("api/Todo")]
    public class TodoController : Controller
    {
        public ITodoRepository TodoItems { get; set; }

        public TodoController(ITodoRepository todoItems)
        {
            TodoItems = todoItems;
        }

        // GET: api/Todo
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.GetAll();
        }

        // GET: api/Todo/5
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = TodoItems.Find(id);
            if (item == null)
                return NotFound();

            return new ObjectResult(item);
        }

        // POST: api/Todo
        [HttpPost]
        public IActionResult Create([FromBody]TodoItem item)
        {
            if (item == null)
                return BadRequest();

            TodoItems.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]TodoItem item)
        {
            if (item == null || item.Key != id)
                return BadRequest();

            TodoItems.Update(item);
            return new NoContentResult();
        }

        // Patch: api/Todo/5
        [HttpPatch("{id}")]
        public IActionResult Update([FromBody]TodoItem item, string id)
        {
            if (item == null)
                return BadRequest();

            var todo = TodoItems.Find(id);

            if (todo == null)
                return NotFound();

            item.Key = todo.Key;

            TodoItems.Update(item);
            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var todo = TodoItems.Find(id);
            if (todo == null)
                return NotFound();

            TodoItems.Remove(id);
            return new NoContentResult();
        }
    }
}
