using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum StoredConversationSpeaker { npc0, npc1, player };

[System.Serializable]
public class StoredConversation {

	/* properties */

	public string title;

	Image NPC0;
	Image NPC1;
	Image Player;

	Color NP0Color;
	Color NP1Color;
	Color PlayerColor;

	public List<string> textList;
	public List<StoredConversationSpeaker> theSpeaker;

	public void initialize() {

		textList = new List<string> ();
		theSpeaker = new List<StoredConversationSpeaker> ();

	}

	public void addBubble(string text, StoredConversationSpeaker spk) {

		textList.Add (text);
		theSpeaker.Add (spk);

	}

	public void setTitle(string t) {

		title = t;

	}

}
