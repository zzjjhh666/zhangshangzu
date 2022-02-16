using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class LoginDTO
    {

        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string VerCode { get; set; }
        public bool IsRemeberMe { get; set; }
    }
}
