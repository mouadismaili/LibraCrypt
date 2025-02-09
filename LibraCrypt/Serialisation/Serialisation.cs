using System;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
namespace LibraCrypt
{
    public interface IBibliothequeSerializer
    {
        void Serialize(BibliothequeData data, string filePath, string key);
        BibliothequeData Deserialize(string filePath, string key);
    }



    public class JsonBibliothequeSerializer : IBibliothequeSerializer
    {
        public void Serialize(BibliothequeData data, string filePath, string key)
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, jsonString);
        }

        public BibliothequeData Deserialize(string filePath, string key)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<BibliothequeData>(jsonString);
        }

    }


    public class XMLBibliothequeSerializer : IBibliothequeSerializer
    {
        public void Serialize(BibliothequeData data, string filePath, string key)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            using (var cryptoStream = CryptoHelper.GetCryptoStream(stream, key, CryptoStreamMode.Write))
            {
                new XmlSerializer(typeof(BibliothequeData)).Serialize(cryptoStream, data);
            }
        }

        public BibliothequeData Deserialize(string filePath, string key)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            using (var cryptoStream = CryptoHelper.GetCryptoStream(stream, key, CryptoStreamMode.Read))
            {
                return (BibliothequeData)new XmlSerializer(typeof(BibliothequeData)).Deserialize(cryptoStream);
            }
        }

    }

    public static class SerializerFactory
    {
        public static IBibliothequeSerializer GetSerializer(string extension)
        {
            return extension.ToLower() switch
            {
                ".json" => new JsonBibliothequeSerializer(),
                ".xml" => new XMLBibliothequeSerializer(),
                _ => throw new NotSupportedException("Format non supporté")
             };
        }
    }

public static class CryptoHelper
{
    public static CryptoStream GetCryptoStream(Stream stream, string key, CryptoStreamMode mode)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] iv = new byte[16];

            if (mode == CryptoStreamMode.Write)
            {
                aes.GenerateIV();
                iv = aes.IV;
                stream.Write(iv, 0, iv.Length);
            }
            else
            {
                stream.Read(iv, 0, iv.Length);
            }

            return new CryptoStream(
                stream,
                mode == CryptoStreamMode.Write ? aes.CreateEncryptor(keyBytes, iv) : aes.CreateDecryptor(keyBytes, iv),
                mode
            );
        }
    }

    private static byte[] GetKeyBytes(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            var sid = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            return Encoding.UTF8.GetBytes(sid);
        }
        return Encoding.UTF8.GetBytes(key);
    }

}
}
