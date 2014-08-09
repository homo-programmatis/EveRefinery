using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpecialFNs
{
	partial class Utility
	{
		// Makes a deep copy of object. Class must be marked as [Serializable]
		public static T CloneUsingBinary<T>(T a_Object)
		{
			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, a_Object);
				stream.Position = 0;

				return (T)formatter.Deserialize(stream);
			}
		}
	}
}