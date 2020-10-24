using System;
using Dapper.Contrib.Extensions;

namespace Modelos
{
    [Table("dbo.Head")]
    public class TodoItem
    {
        [ExplicitKey]
        public long Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public DateTime reminder { get; set; }
        public long checklist { get; set; }
    }
}
