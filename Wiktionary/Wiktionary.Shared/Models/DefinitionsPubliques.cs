using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace Wiktionary.Models
{
    public class DefinitionsPubliques
    {
         private string word;
        private string definition;

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
                return word;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                word = value;
            }
        }

        public string Definition
        {
            get
            {
                return definition;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                definition = value;
            }
        }
    }
}
