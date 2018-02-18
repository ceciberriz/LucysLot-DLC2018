using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	public static TextManager instance;

	public GameObject textPrefab;
	public float textYOffset = 10f;

	public float textDelay = 2f;
	public float textScrollSpeed = 5f;

	bool npcTalking = false;
	bool playerTalking = false;
    bool inConversation = false;
	Coroutine pTalkRoutine;

	private Conversation conversation = new Conversation();
	private Queue<GameObject> dialogueBoxes;
	private GameObject lastDialogueBox;
	private int boxCounter = 1;
	private bool isScrolling;

	public Canvas uiCanvas;
	private Text playerText;
	public Text npcText;
	private Button pButton;
	public GameObject pButtonPrefab;
	public GameObject npcPanel;
	public GameObject npcPanelPrefab;
	public float panelYOffset;

	private PlayerMessage currPlayerOption;
	private PlayerMessage[] currPResponses;
	private bool optionSelected;

	private Stack<NPCMessage> NPCMessageStack = new Stack<NPCMessage>();

	private void Awake()
	{
		//Make sure there is only one instance of the TextManager
		if (instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		dialogueBoxes = new Queue<GameObject>();
		dialogueBoxes.Enqueue(npcPanel);
		lastDialogueBox = npcPanel; //Used to determine position of new dialogue boxes.  

		//Take messages from Conversation and push to stack, in reverse b/c FILO
		NPCMessage[] messages = conversation.getMessages();
		for (int i = messages.Length - 1; i >= 0; i--)
		{
			NPCMessageStack.Push(messages[i]);
		}
	}

	void Update()
	{
		//NpcTalking is true when NPC conversation is initiated by selecting NPC. 
		if (Input.GetMouseButtonDown(0) && npcTalking)
		{
			//Displays next npc message by popping from stack. 
			//Cannot happen if playerTalking is true(player options are showing).
			NPCTalk();
		}
		else if(Input.GetMouseButtonDown(0) && !npcTalking && playerTalking)
		{
			//Create button that will display player options when clicked. 
			pButton = AddNewDialogueBox(pButtonPrefab).GetComponent<Button>(); //Add player button to dialogue box queue.
			pButton.onClick.AddListener(StartPlayerTalk); //Add listener to start displaying player options.
			playerText = pButton.GetComponentInChildren<Text>(); //Store text of new player button.
			if (boxCounter > 2) //If more than two dialogue boxes, scroll them and remove top box.
			{
				StartCoroutine(ScrollAllDialogue());
			}
			playerTalking = false; //Prevents adding new player buttons while there is a player button.
		}
	}

	//Displays next npc message by popping from stack. 
	//Cannot happen if playerTalking is true(player options are showing)
	void NPCTalk()
	{
		NPCMessage currMessage = null; //To store popped message.
		if(NPCMessageStack.Count > 0) //If any remaining messages in the stack.
		{
			currMessage = NPCMessageStack.Pop(); //Pop and store message.
		}
		if(currMessage != null) //If there is a popped message.
		{
			GameObject newNPCPanel = AddNewDialogueBox(npcPanelPrefab); //Instantiate new NPC dialogue box. 
			Text newNPCText = newNPCPanel.GetComponentInChildren<Text>(); //Store text of new NPC dialogue box.
			newNPCText.text = currMessage.getText(); //Replace text with current message text.
			StartCoroutine(ScrollAllDialogue()); //Scroll all dialogue boxes. 
			if (currMessage.hasResponses()) //If player has responses to npc message.
			{
				playerTalking = true; //Can add new player buttons. 
				currPResponses = currMessage.getResponses(); //Store player response array. 
				npcTalking = false; //Set to false so we don't show new npc dialogues while player options are showing. 
				//playerTalking = true; //Can add new player button.           
			}
		}                
	}

	//Scroll all dialogue boxes. 
	IEnumerator ScrollAllDialogue()
	{
		boxCounter--; //Substract number of boxes, determines whether all dialogue boxes should be scrolled. 
		float currDistanceTravelled = 0; //Calculate current distance travelled per loop.
		while (currDistanceTravelled < panelYOffset) //While current distance travelled is not full. 
		{
			foreach (GameObject box in dialogueBoxes)
			{
				box.transform.Translate(Vector2.up * textScrollSpeed * Time.deltaTime); //Scroll all boxes up.
			}
			currDistanceTravelled += (Vector2.up * textScrollSpeed * Time.deltaTime).y; //Increment currDistanceTravelled.
			yield return null;
		}
		Destroy(dialogueBoxes.Dequeue()); //Dequeue and destroy the now off-screen dialogue box. 
	}

	//Button has a listener for this function, which will call PlayerTalk Coroutine.
	private void StartPlayerTalk()
	{
		pTalkRoutine = StartCoroutine(PlayerTalk(currPResponses));
	}

	//Display player responses with a delay between each message.
	IEnumerator PlayerTalk(PlayerMessage[] messages)
	{
		pButton.onClick.RemoveAllListeners(); //Remove the StartPlayerTalk listener and replace with OptionSelected.
		pButton.onClick.AddListener(OptionSelected); //OptionSelected will tell textmanager what option player has selected.
		foreach (PlayerMessage message in messages) //Loop through player messages.
		{
			pButton.interactable = false; //Set button to not clickable so that it can't be clicked while scrolling.
			StartCoroutine(ScrollPlayerOptions(playerText.gameObject, message.getText())); //Scroll player options.
			currPlayerOption = message; //Store current message in order to tell NPC what was selected. 
			yield return new WaitForSeconds(textDelay); //This delays displaying the next player text.
			if(message == messages[messages.Length - 1]) //If it has been two seconds and current message is last player option.
			{
				OptionSelected(); //Select current option.
			}
		}
	}

	//Scrolls player options within a player button. Stores current player text. 
	IEnumerator ScrollPlayerOptions(GameObject originalText, string newText)
	{
		//Create next player text option under the current one.
		GameObject pTextObject = Instantiate(textPrefab, new Vector2(playerText.transform.position.x, playerText.transform.position.y - textYOffset),
			transform.rotation, pButton.transform);
		Text newTextObject = pTextObject.GetComponent<Text>(); //Store Text in variable.
		newTextObject.text = newText; //Replace visible text. 
		float currDistanceTravelled = 0; //Initialize variable to track distance scrolled by player options.
		Vector2 startLoc = pTextObject.transform.position; //Starting location of instanitated text. 
		while (currDistanceTravelled < textYOffset) //While options haven't travelled full distance. 
		{
			originalText.transform.Translate(Vector2.up * textScrollSpeed * Time.deltaTime); //Scroll previous text over time.
			pTextObject.transform.Translate(Vector2.up * textScrollSpeed * Time.deltaTime); //Scroll new text over time. 
			currDistanceTravelled = ((Vector2)pTextObject.transform.position - startLoc).magnitude; //Calculate distance travelled.
			yield return null; //Yield to Unity before restarting loop.
		}
		pButton.interactable = true; //Set Button to clickable again.
		playerText = newTextObject; //Store new text as current text object.
		Destroy(originalText); //Destroy previous text. 
	}

	//Tell npc what option player selected and stop displaying player options.
	public void OptionSelected()
	{
		//playerTalking = false; //Player is no longer talking. Can't add new player buttons.
		pButton.interactable = false; //Set button to not clickable. 
		if (!npcTalking) //Make sure the conversation is in player options mode.
		{
			StopCoroutine(pTalkRoutine); //Stop displaying player messages.
			npcTalking = true; //Set to false so npc dialogue can be cycled through again.
			if (currPlayerOption.hasResponse()) //If npc has responses to player choice.
			{
				NPCMessage[] messages = currPlayerOption.getResponse(); //get npc responses to player choice.
				for(int i = messages.Length - 1; i >= 0; i--) //loop backwards through npc responses.
				{
					//Push npc responses onto stack, thus setting priority to the newest npc messages.
					//Since older npc responses(from previous dialogue branches) are still stored, npc will eventually loop through those as well.
					//If we want to end conversation at a player choice or npc dialogue, we should add an XML tag like "end conversation".
					NPCMessageStack.Push(messages[i]); 
				}
				NPCTalk(); //Display next npc message, checking for more player options, etc.                
			}
		}
	}

	//Needed to start conversation called by NPC class.
	public void setNPC(GameObject clickedNPC)
	{
        if (!inConversation)
        {
            npcTalking = true; //Update checks for this bool to start conversation.
            inConversation = true;
        }
	}

	//Instantiates a new dialogue box below the previous. Takes in a GameObject so it can either be Panel or Button. 
	//Enqueue onto queue, store last dialogue box. 
	private GameObject AddNewDialogueBox(GameObject prefab)
	{
		GameObject obj = Instantiate(prefab, new Vector2(lastDialogueBox.transform.position.x, lastDialogueBox.transform.position.y - panelYOffset),
			transform.rotation, uiCanvas.transform);
		dialogueBoxes.Enqueue(obj);
		lastDialogueBox = obj;
		boxCounter++;
		return obj;
	}
}