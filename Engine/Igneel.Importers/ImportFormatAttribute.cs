using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Importers
{
    public class ImportFormatAttribute : Attribute
    {
        public ImportFormatAttribute(string fileExtension)
        {
            this.FileExtension = fileExtension;
        }
        public string FileExtension { get; set; }
    }
}
