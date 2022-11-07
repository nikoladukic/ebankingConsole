using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBanking.Console.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{this.Id} {this.FirstName} {this.LastName}";
        }
    }
    public class Currency
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

}
