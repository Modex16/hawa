using Facebook;
using Facebook.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whereAir
{
    public static class User
    {
        public static FBSession FBAccount { get; set; }

        public static string Name
        {
            get
            {
                return FBAccount.User.Name;
            }
        }

        /*public static string Email
        {
            get
            {
                return FBAccount.User.
            }
        }*/

    }
}
