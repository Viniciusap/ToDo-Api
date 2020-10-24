using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modelos;
using Microsoft.Extensions.Configuration;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;
        string myDb1ConnectionString;

        public TodoController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            if (_context.TodoItems.Count() != 0)
            {
                foreach (var entity in _context.TodoItems)
                    _context.TodoItems.Remove(entity);
                _context.SaveChanges();
            }

            myDb1ConnectionString = _configuration.GetConnectionString("myDb1");

            SqlConnection sqlConn = new SqlConnection(myDb1ConnectionString);

            sqlConn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Head ORDER BY  id", sqlConn);

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                _context.TodoItems.Add(new TodoItem
                {
                    Id = int.Parse(reader["id"].ToString()),
                    title = reader["title"].ToString(),
                    description = reader["description"].ToString(),
                    date = DateTime.Parse(reader["date"].ToString()),
                    reminder = DateTime.Parse(reader["reminder"].ToString()),
                    checklist = int.Parse(reader["checklist"].ToString())
                });
                _context.SaveChanges();
            }


            sqlConn.Close();

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { title = "Item1" });
                _context.TodoItems.Add(new TodoItem { title = "Item2" });
                _context.TodoItems.Add(new TodoItem { title = "Item3" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        [HttpPost]
        public bool InsertTodoItem(TodoItem todoItem)
        {
            SqlConnection sqlConn = new SqlConnection(myDb1ConnectionString);

            sqlConn.Open();

            SqlCommand cmd = new SqlCommand("Insert into Head (id,title,description,date,reminder,checklist) VALUES ( " +
                todoItem.Id +
                "," +
                Quoted(todoItem.title) +
                "," +
                Quoted(todoItem.description) +
                "," +
                Quoted(todoItem.date.ToString()) +
                "," +
                Quoted(todoItem.reminder.ToString()) +
                "," +
                todoItem.checklist + ")", sqlConn);

            bool i = Convert.ToBoolean(cmd.ExecuteNonQuery());

            sqlConn.Close();

            return i;
        }

        [HttpGet("api/teste")]
        public TodoItem GetDetalhesEstado(string id)
        {
            using (SqlConnection conexao = new SqlConnection(myDb1ConnectionString))
            {
                return conexao.Get<TodoItem>(id);
            }
        }
        [HttpPost("api/teste2")]
        public bool InsertTdItens(TodoItem todoItem)
        {
            using (SqlConnection conexao = new SqlConnection(myDb1ConnectionString))
            {
                Console.WriteLine("Teste");
                conexao.Open();
                var identity = conexao.Insert(todoItem);
                return Convert.ToBoolean(identity);
            }            
        }
        private string Quoted(string str)
        {
            string qt = "'";
            return qt + str + qt;
        }
    }

}