using System;
using Dapper.Contrib.Extensions;

namespace Modelos
{
    [Table("dbo.Cliente")]
    public class Cliente
    {
        [ExplicitKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Profession { get; set; }
    }
}
