using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code.Enum
{
    public class EnumTextValue : Attribute
    {
        public EnumTextValue(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
