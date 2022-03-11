using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using T.Data;
using T.Models;
using T.ViewModels;

namespace api_balta_sqlite.Controllers
{
    [ApiController]//identifica que é um controller de api
    [Route("v1")]//define um prefixo no caso v1/todos
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDBContext ctx)
        {
            //IActionResult é um tipo padrão
            var todos = await ctx
                .Todos
                /* AsNoTracking é um item de performace, 
                 * tras velocidade pois não monitora as 
                 * operações feitas nos registros */
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }
        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDBContext ctx,
            [FromRoute] int id
            )
        {
            var todo = await
                ctx
                .Todos
                .FirstOrDefaultAsync(x => x.Id == id);

            return todo != null ? Ok(todo) : BadRequest();
        }

        [HttpPost("todos")]//podemos definir a rota no verbo http
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDBContext ctx,
            [FromBody] CreateTodoViewModel model
            )
        {
            //ModelState verifica se a validação que foi definida no viewmodel está sendo atendida
            if (!ModelState.IsValid)
                return BadRequest();
            
            
            var todo = new Todo
            {
                Title = model.Title,
                Done = false,
                Date = DateTime.Now,
            };

            try
            {
                //O .AddAsync salva a instancia em mémoria
                await ctx.Todos.AddAsync(todo);
                //O .SaveChangesAsync salva a instancia no Banco
                await ctx.SaveChangesAsync();

                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPut("todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDBContext ctx,
            [FromBody] UpdateViewModel model,
            [FromRoute] int id
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = await ctx
                .Todos
                .FirstOrDefaultAsync(x => x.Id == id);

            if(todo == null)
            {
                return NotFound();
            }

            try
            {
                todo.Title = model.Title;

                ctx.Todos.Update(todo);
                await ctx.SaveChangesAsync();

                return Ok(todo);

            }catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDBContext ctx,
            [FromRoute] int id
            )
        {
            var todo = await ctx.Todos.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                ctx.Todos.Remove(todo);
                await ctx.SaveChangesAsync();

                return Ok();
            }catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}