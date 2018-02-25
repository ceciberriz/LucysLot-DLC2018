using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;

[XmlRoot("Conversation")]
public class Conversation{

    [XmlArray("messages")]
    [XmlArrayItem("NPCMessage")]
    public NPCMessage[] messages;

    public NPCMessage[] getMessages()
    {
        return messages;
    }

    public string toString()
    {
        var myString = "Conversation";
        foreach (NPCMessage msg in messages)
        {
            myString = myString + "\n  " + msg.toString();
        }
        return myString;
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(Conversation));
        using(var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static Conversation Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Conversation));
        using(var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Conversation;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Conversation LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Conversation));
        return serializer.Deserialize(new StringReader(text)) as Conversation;
    }
}
