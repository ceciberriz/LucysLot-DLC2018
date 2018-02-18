using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerMessage
{
	private string text;
	private NPCMessage[] response;

	public PlayerMessage(string txt, NPCMessage[] resp)
	{
		text = txt;
		response = resp;
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
