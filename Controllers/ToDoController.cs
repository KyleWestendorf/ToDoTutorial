using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;

            if (_context.ToDoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.ToDoItems.Add(new ToDoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

            // GET: api/ToDo
            [HttpGet]
            public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTodoItems()
            {
                return await _context.ToDoItems.ToListAsync();
            }

            // GET: api/ToDo/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ToDoItem>> GetTodoItem(long id)
            {
                var todoItem = await _context.ToDoItems.FindAsync(id);

                if (todoItem == null)
                {
                    return NotFound();
                }

                return todoItem;
            }

            // POST: api/ToDo
            [HttpPost]
            public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem toDoItem)
            {
                _context.ToDoItems.Add(toDoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTodoItem", new { id = toDoItem.Id }, toDoItem);

            }

            // PUT: api/Todo/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutTodoItem(long id, ToDoItem toDoItem)
            {
                if (id != toDoItem.Id)
                {
                    return BadRequest();
                }

                _context.Entry(toDoItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/Todo/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<ToDoItem>> DeleteTodoItem(long id)
            {
                var todoItem = await _context.ToDoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }

                _context.ToDoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                return todoItem;
            }
    }
}

