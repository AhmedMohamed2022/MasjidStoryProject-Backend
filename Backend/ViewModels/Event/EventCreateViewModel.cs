﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EventCreateViewModel
    {
        public DateTime EventDate { get; set; }
        public int? MasjidId { get; set; }
        public List<EventContentViewModel> Contents { get; set; } = new();
    }

}
