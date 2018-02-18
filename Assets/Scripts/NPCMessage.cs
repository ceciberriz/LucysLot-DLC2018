using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;

public class NPCMessage
{
	private string text;
	private PlayerMessage[] responses;

	public NPCMessage(string txt, PlayerMessage[] resp)
	{
		text = txt;
		responses = resp;
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