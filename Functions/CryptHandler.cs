using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UGC_API.Functions
{
    class CryptHandler
    {
        internal static string HashPasword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: BCrypt.Net.HashType.SHA384);
        }

        internal static bool CheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash, hashType: BCrypt.Net.HashType.SHA384);
        }
    }
}
