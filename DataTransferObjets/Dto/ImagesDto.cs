using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjets.Dto
{
    public class ImagesDto
    {
        public IFormFileCollection Files { get; set; }
        public string UploadPath { get; set; }
    }
}
