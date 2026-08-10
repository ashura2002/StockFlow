using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message):base(message)
        {

        }
    }
}
