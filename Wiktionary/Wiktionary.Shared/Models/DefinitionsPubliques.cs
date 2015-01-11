using System;
using Windows.Data.Json;

namespace Wiktionary.Models
{
    public class DefinitionsPubliques
    {
         private string _word;
        private string _definition;

        public DefinitionsPubliques()
        {
            Word = "";
            Definition = "";
        }

        public DefinitionsPubliques(JsonObject jsonObject)
        {
            Word = jsonObject.GetNamedString("Word");
            Definition = jsonObject.GetNamedString("Definition");
        }
        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _word = value;
            }
        }

        public string Definition
        {
            get
            {
                return _definition;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _definition = value;
            }
        }
    }
}
