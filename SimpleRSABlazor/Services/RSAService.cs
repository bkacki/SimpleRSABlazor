using SimpleRSA;

namespace SimpleRSABlazor.Services
{
    public interface IRSAService
    {
        public string EncryptMessage(RSAKey rsaKey, string message);
        public string DecryptMessage(RSAKey rsaKey, string message);
        public RSAKey GenerateKeys();
    }

    public class RSAService : IRSAService
    {
        public string EncryptMessage(RSAKey rsaKey, string message)
        {
            return RSA.EncryptMessage(rsaKey, message);
        }

        public string DecryptMessage(RSAKey rsaKey, string message)
        {
            return RSA.DecryptMessage(rsaKey, message);
        }

        public RSAKey GenerateKeys()
        {
            return new RSAKey();
        }
    }
}
