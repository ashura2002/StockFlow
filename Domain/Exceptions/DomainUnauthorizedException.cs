using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class DomainUnauthorizedException:Exception
    {
        public DomainUnauthorizedException(string message):base(message)
        {
            
        }
    }
}
