using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldenFarm.Web.Models
{
    public class UserIndexViewModel
    {
        public User User { get; set; }

        public IEnumerable<UserProduct> UserProducts { get; set;
        }
    }


    public class PopDetailViewModel
    {
        public int DirectRefUsersCount { get; set; }

        public int IndirectRefUsersCount { get; set; }

        public IEnumerable<User> RefUsers { get; set; }
    }
}