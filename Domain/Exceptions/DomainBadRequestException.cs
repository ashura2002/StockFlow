using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class DomainBadRequestException:Exception
    {
        public DomainBadRequestException(string message) : base(message) { }
    }
}
