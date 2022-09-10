using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs
{
    public class UploadFileDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
