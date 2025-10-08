using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }  
        public string? Document { get; set; }
        public string? PhotoURL { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<Sale>? Sales { get; set; }
    }
}
