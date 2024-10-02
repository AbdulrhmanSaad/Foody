﻿using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.Dto
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

    }
}
