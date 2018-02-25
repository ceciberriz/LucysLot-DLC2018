using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    private void OnMouseDown()
    {
        TextManager.instance.setNPC(this.gameObject);
    }
}
