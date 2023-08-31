using HashGenerator.Interfaces;
using SHA3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGenerator.Processors
{
    internal class HashGenerator : IHashGenerator
    {
        public string GenerateHash(string messageToHash)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(messageToHash);

            SHA3Managed sha3 = new SHA3Managed(256);
            byte[] hash = sha3.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();

        }
    }
}
