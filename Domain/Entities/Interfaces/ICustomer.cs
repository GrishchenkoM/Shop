﻿using System;

namespace Domain.Entities.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        DateTime CreatedDate { get; set; }

        string Address { get; set; }

        string Sex { get; set; }

        string Phone { get; set; }

        string Email { get; set; }
    }
}
