using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DTOAccount
    {
        [Required]
        public string un { get; set; }

        [Required]
        public string pw { get; set; }
        
    }
}