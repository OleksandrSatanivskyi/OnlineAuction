﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortuneWheel.Domain.Auth
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
