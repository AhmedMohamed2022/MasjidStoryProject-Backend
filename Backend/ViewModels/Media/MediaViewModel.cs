using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class MediaViewModel
    {
        public string Url { get; set; }
        public string MediaType { get; set; }  // Image or Video
    }

}
