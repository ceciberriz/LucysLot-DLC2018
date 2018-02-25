using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class NPCMessage
{
    [XmlAttribute("text")]
    public string text;

    [XmlArray("responses")]
    [XmlArrayItem("PlayerMessage")]
    public PlayerMessage[] responses;

    //public NPCMessage(string txt, PlayerMessage[] resp)
    //{
    //    text = txt;
    //    responses = resp;
    //}

    public string toString()
    {
        var myString = ("NPCMsg: " + this.text);
        foreach (PlayerMessage msg in responses)
        {
            myString = myString + "\n  " + msg.toString();
        }
        return myString;
    }

    public bool hasText()
    {
        return text != "";
    }

    public string getText()
    {
        return text;
    }

    public bool hasResponses()
    {
        return responses != null;
    }

    public PlayerMessage[] getResponses()
    {
        return responses;
    }
   
}