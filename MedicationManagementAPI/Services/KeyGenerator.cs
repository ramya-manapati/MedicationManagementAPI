using System.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;
namespace MedicationManagementAPI.Services
{
    public class KeyGenerator
    {
        public static string GenerateJwtKey(int byteSize = 64)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[byteSize];
                rng.GetBytes(key);  
                return Convert.ToBase64String(key);  
            }
        }
    }
}


    
