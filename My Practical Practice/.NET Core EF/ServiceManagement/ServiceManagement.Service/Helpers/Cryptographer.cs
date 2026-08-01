using System;
using System.Security.Cryptography;
using System.Text;

namespace ServiceManagement.Service.Helpers
{
	public class Cryptographer
	{
		private const string SecretKey = "ServiceManagement@12345@12345@#$";

		// Static salt instead of generating a random byte array each time
		private static readonly byte[] StaticSalt = Encoding.UTF8.GetBytes("ServiceMgmtStaticSalt2026!");

		private const int KeySize = 32; // 256 bits
		private const int Iterations = 350000;
		private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

		public static string EncryptPassword(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				return string.Empty;
			}

			// Combine password with secret key
			byte[] keyedPassword = Encoding.UTF8.GetBytes(password + SecretKey);

			// Derive key using PBKDF2 with the STATIC salt
			byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
				keyedPassword,
				StaticSalt,
				Iterations,
				HashAlgorithm,
				KeySize
			);

			// Always yields the exact same Base64 string for the same password
			return Convert.ToBase64String(hash);
		}
	}
}