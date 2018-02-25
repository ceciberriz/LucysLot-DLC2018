using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerMessage
{
    [XmlAttribute("text")]
    public string text;

    [XmlArray("response")]
    [XmlArrayItem("NPCMessage")]
    public NPCMessage[] response;

    //public PlayerMessage(string txt, NPCMessage[] resp)
    //{
    //    text = txt;
    //    response = resp;
    //}

    public string toString()
    {
        var myString = ("PLRMsg: " + this.text);
        foreach (NPCMessage msg in response)
        {
            myString = myString + "\n  " + msg.toString();
        }
        return myString;
    }

    public bool hasText()
    {
        return text == "";
    }

    public string getText()
    { 
        return text;
    }

    public bool hasResponse()
    {
        return response != null;
    }

    public NPCMessage[] getResponse()
    {
        return response;
    }
}
