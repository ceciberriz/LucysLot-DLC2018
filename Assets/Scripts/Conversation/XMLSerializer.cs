using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    void Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Conversation));
        var stream = new FileStream(path, FileMode.Open);
        var conversation = serializer.Deserialize(stream) as Conversation;
        stream.Close();

        Debug.Log(conversation);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
