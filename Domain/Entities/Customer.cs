﻿using System;
using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Address { get; set; }

        public string Sex { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
