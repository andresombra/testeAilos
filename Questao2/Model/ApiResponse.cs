﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Model
{
    public class ApiResponse
    {
        public List<FootballMatch> data { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
        public int page { get; set; }
        public int total_pages { get; set; }
    }
}
