using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SpecialFNs
{
	[Serializable]
	[XmlRoot("dictionary")]
	public class SerializableDictionary<TKey, TValue>
		: Dictionary<TKey, TValue>, IXmlSerializable
	{
		public SerializableDictionary()
		{

		}

		// Support for [Serializable]
		public SerializableDictionary(SerializationInfo a_Info, StreamingContext a_Context) : base(a_Info, a_Context)
		{

		}

		#region XML Serialization rules
		private void ReadXml_Simple(System.Xml.XmlReader reader)
		{
			bool hasItem = reader.ReadToDescendant("item");
			if (!hasItem)
			{
				reader.Skip();
				return;
			}

			for (; hasItem; hasItem = reader.ReadToNextSibling("item"))
			{
				TKey key = (TKey)Convert.ChangeType(reader.GetAttribute("key"), typeof(TKey));
				TValue value = (TValue)Convert.ChangeType(reader.GetAttribute("value"), typeof(TValue));
				this.Add(key, value);
			}

			reader.ReadEndElement();
		}

		private void WriteXml_Simple(System.Xml.XmlWriter writer)
		{
			foreach (TKey key in this.Keys)
			{
				writer.WriteStartElement("item");

				writer.WriteStartAttribute("key");
				writer.WriteValue(key.ToString());
				writer.WriteEndAttribute();

				writer.WriteStartAttribute("value");
				TValue value = this[key];
				writer.WriteValue(value.ToString());
				writer.WriteEndAttribute();

				writer.WriteEndElement();
			}
		}

		private void ReadXml_Complex(System.Xml.XmlReader reader)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();
			if (wasEmpty)
				return;

			while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");

				reader.ReadStartElement("key");
				TKey key = (TKey)keySerializer.Deserialize(reader);
				reader.ReadEndElement();

				reader.ReadStartElement("value");
				TValue value = (TValue)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();

				this.Add(key, value);

				reader.ReadEndElement();
				reader.MoveToContent();
			}

			reader.ReadEndElement();
		}

		private void WriteXml_Complex(System.Xml.XmlWriter writer)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			foreach (TKey key in this.Keys)
			{
				writer.WriteStartElement("item");

				writer.WriteStartElement("key");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();

				writer.WriteStartElement("value");
				TValue value = this[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
		}
		#endregion

		bool CanUseSimpleFormat()
		{
			var typeConvK = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TKey));
			if (!typeConvK.CanConvertFrom(typeof(String)) || !typeConvK.CanConvertTo(typeof(String)))
				return false;

			var typeConvV = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TValue));
			if (!typeConvV.CanConvertFrom(typeof(String)) || !typeConvV.CanConvertTo(typeof(String)))
				return false;
			
			return true;
		}

		#region IXmlSerializable Members
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			if (CanUseSimpleFormat())
				ReadXml_Simple(reader);
			else
				ReadXml_Complex(reader);
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			if (CanUseSimpleFormat())
				WriteXml_Simple(writer);
			else
				WriteXml_Complex(writer);
		}
		#endregion
	}
}