using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IPasswordService
    {
       string HashPassword(string plainText);
        bool Verify(string password, string hashPassword);
    }
}
