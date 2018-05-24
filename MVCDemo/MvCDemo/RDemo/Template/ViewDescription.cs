using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Template
{
    public class FormateAttribute : Attribute
    {
        public FormateAttribute(string format)
        {
            this.Format = format;    
        }
        public string GetMessage(string value)
        {
            return string.Format(Format, value);
        }
        protected string Format { get; }
    }
}
