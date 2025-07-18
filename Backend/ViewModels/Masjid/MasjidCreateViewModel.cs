﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViewModels
{
    public class MasjidCreateViewModel
    {
        [Required]
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string ArchStyle { get; set; }
        public string Description { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int CityId { get; set; }
        public int? YearOfEstablishment { get; set; }

        // Media support
        public List<IFormFile>? MediaFiles { get; set; }
    }
}