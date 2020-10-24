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
    public class ClienteController : ControllerBase
    {
        private readonly ClienteContext _context;
        private readonly IConfiguration _configuration;
        string myDb1ConnectionString;

        public ClienteController(ClienteContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            myDb1ConnectionString = _configuration.GetConnectionString("myDb1");


            if (_context.Clientes.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.Clientes.Add(new Cliente { Id = 1 });
                _context.Clientes.Add(new Cliente { Id = 2 });
                _context.Clientes.Add(new Cliente { Id = 3 });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpPost]
        public bool InsertCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            bool i = true;

            return i;
        }
    }
}
