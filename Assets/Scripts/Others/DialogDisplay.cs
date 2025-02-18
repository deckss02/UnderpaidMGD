using UnityEngine;

public class DialogDisplay : MonoBehaviour {
	public Conversation conversation;
	public GameObject speakerLeft;
	public GameObject speakerRight;

	SpeakerUI speakerUILeft;
	SpeakerUI speakerUIRight;

	int activeLineIndex = 0;

	void Start() {
		speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
		speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

		speakerUILeft.Speaker = conversation.speakerLeft;
		speakerUIRight.Speaker = conversation.speakerRight;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			AdvanceConversation();
		}
	}

	void AdvanceConversation() {
		if(activeLineIndex < conversation.lines.Length) {
			DisplayLine();
			activeLineIndex += 1;
		} else {
			speakerUILeft.Hide();
			speakerUIRight.Hide();
			activeLineIndex = 0;
		}
	}

	void DisplayLine() {
		Line line = conversation.lines[activeLineIndex];
		//Sprite portrait = conversation.portrait[0];
		Character Character = line.character;

		if(speakerUILeft.SpeakerIs(Character)) {
			SetDialog(speakerUILeft, speakerUIRight, line.text);
		} else {
			SetDialog(speakerUIRight, speakerUILeft, line.text);
		}
	}

	void SetDialog(SpeakerUI activeSpeakerUI, SpeakerUI inactiveSpeakerUI, string text) {
		activeSpeakerUI.Dialog = text;
		activeSpeakerUI.Show();
		inactiveSpeakerUI.Hide();
	}
}