using System;
using System.Collections.Generic;

#nullable disable

namespace Ljekobilje
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public int? Language { get; set; }
        public int? Theme { get; set; }
    }
}
