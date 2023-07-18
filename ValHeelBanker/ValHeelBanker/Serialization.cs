using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ValHeelBanker
{
  internal class Serialization
  {
    private static string UTF8ByteArrayToString(Byte[] characters)
    {
      UTF8Encoding encoding = new UTF8Encoding();
      String constructedString = encoding.GetString(characters);
      return (constructedString);
    }

    private static Byte[] StringToUTF8ByteArray(String xmlString)
    {
      UTF8Encoding encoding = new UTF8Encoding();
      Byte[] byteArray = encoding.GetBytes(xmlString);
      return byteArray;
    }

    public static string Serialize<TObjectType>(TObjectType objectToSerialize)
    {
      try
      {
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(TObjectType));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        xs.Serialize(xmlTextWriter, objectToSerialize);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        return UTF8ByteArrayToString(memoryStream.ToArray());
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return string.Empty;
      }
    }

    public static TObjectType Deserialize<TObjectType>(string xmlString)
    {
      XmlSerializer xs = new XmlSerializer(typeof(TObjectType));
      MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
      XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
      return (TObjectType)xs.Deserialize(memoryStream);
    }
  }
}
