using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Wiktionary.Models
{
    [Table("Definitions")]
    public class DefinitionsTable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string Mot { get; set; }

        public string Definition { get; set; }
    }
}
