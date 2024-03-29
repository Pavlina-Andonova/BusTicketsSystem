﻿namespace PandaTour.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
