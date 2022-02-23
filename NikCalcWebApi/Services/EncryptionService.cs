using System.Security.Cryptography;
using System.Text;

namespace NikCalcWebApi.Services;

public class EncryptionService
{
    private readonly IConfiguration _configuration;

    public EncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Encrypt(string text)
    {
        string EncryptionKey = _configuration["Secret"];
        byte[] clearBytes = Encoding.Unicode.GetBytes(text);
        string result;
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
            }
            result = Convert.ToBase64String(ms.ToArray());
        }
        return result;
    }
}
