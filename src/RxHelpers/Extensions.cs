using System.IO;
using System.Text;
using Hyperion;

// ReSharper disable once CheckNamespace
namespace System
{
	public static class Extensions
	{
		private static readonly Serializer Serializer = new Serializer();

		public static string ToBase64(this byte[] bytes) => Convert.ToBase64String(bytes);
		public static byte[] FromBase64(this string text) => Convert.FromBase64String(text);
		public static byte[] ToUtf8(this string text) => Encoding.UTF8.GetBytes(text);
		public static string FromUtf8(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

		public static byte[] Pickle(this object subject)
		{
			using (var mem = new MemoryStream())
			{
				Serializer.Serialize(subject, mem);
				return mem.ToArray();
			}
		}

		public static T Unpickle<T>(this byte[] bytes)
		{
			using (var mem = new MemoryStream(bytes))
			{
				return Serializer.Deserialize<T>(mem);
			}
		}
	}
}
