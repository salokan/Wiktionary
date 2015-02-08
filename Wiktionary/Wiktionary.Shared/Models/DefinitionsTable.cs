using SQLite;

//Ce fichier permet de faire la liaison avec la base sqlite

namespace Wiktionary.Models
{
    [Table("Definitions")]
    public class DefinitionsTable
    {
        public DefinitionsTable()
        {

        }
        public DefinitionsTable(int _id)
        {
            id = _id;
        }

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string Mot { get; set; }

        public string Definition { get; set; }
    }
}