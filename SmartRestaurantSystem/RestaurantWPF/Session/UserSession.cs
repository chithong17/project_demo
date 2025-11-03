using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.Session
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; }
        public static string Username { get; set; }
        public static string RoleName { get; set; }

        public static string Phone { get; set; }

        public static bool IsLoggedIn => UserId > 0;

        public static void Clear()
        {
            UserId = 0;
            FullName = null;
            Username = null;
            RoleName = null;
            Phone = null;
        }
    }
}
