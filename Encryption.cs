using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.IO;

namespace AutomachefDataEditor
{
    class Encryption
    {
		//A constant salt that's used in generating the password for en/decryption.
		const string PASSWORD_SALT = "hewasindeedandtaforthisimpl";

		/// <summary>
		/// Calculates the unique Platform ID for this machine based on the algorithm used in Automachef. This is derived
		/// from Unity's own device UID algorithm, specific to different platforms.
		/// Also present in the folder name containing the save files.
		/// </summary>
		/// <returns></returns>
		public static string GetPlatformUID()
		{
			return null;
			//return Convert.ToBase64String(Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier));
		}

		public static string Decrypt(string CipherText, string platformUID, string Salt = "the salty tears provide thy nourishment", string HashAlgorithm = "SHA1", int PasswordIterations = 2, int KeySize = 256)
		{
			string password = PASSWORD_SALT + platformUID; //Previous GetPassword algorithm inlined

			if (string.IsNullOrEmpty(CipherText))
			{
				return "";
			}
			char[] array = new char[16];
			string value = "Ver:";
			if (!CipherText.StartsWith(value))
			{
				return CipherText;
			}

			int num = 0;
			int num2 = CipherText.IndexOf(':') + 1;
			int num3 = CipherText.IndexOf('\n');
			if (CipherText.StartsWith(value))
			{
				int.TryParse(CipherText.Substring(num2, num3 - num2), out num);
			}
			switch (num)
			{
				case 0:
					array = "j()v5hDFaCS()pxh".ToCharArray();
					break;
				case 1:
					array = CipherText.Substring(num3 + 1, 16).ToCharArray();
					CipherText = CipherText.Substring(num3 + 1 + 16);
					break;
				default:
					throw new Exception("unknown version for encrypted file.");
			}
			try
			{
				byte[] bytes = Encoding.ASCII.GetBytes(array);
				byte[] bytes2 = Encoding.ASCII.GetBytes(Salt);
				byte[] array2 = Convert.FromBase64String(CipherText);
				byte[] bytes3 = new PasswordDeriveBytes(password, bytes2, HashAlgorithm, PasswordIterations).GetBytes(KeySize / 8);
				RijndaelManaged rijndaelManaged = new RijndaelManaged();
				rijndaelManaged.Mode = CipherMode.CBC;
				byte[] array3 = new byte[array2.Length];
				using (ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes3, bytes))
				{
					using (MemoryStream memoryStream = new MemoryStream(array2))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
						{
							cryptoStream.Read(array3, 0, array3.Length);
							memoryStream.Close();
							cryptoStream.Close();
						}
					}
				}
				rijndaelManaged.Clear();
				if (array3 != null)
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						memoryStream2.Write(array3, 0, array3.Length);
						memoryStream2.Seek(0L, SeekOrigin.Begin);
						return binaryFormatter.Deserialize(memoryStream2) as string;
					}
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			return null;
		}
	}
}
