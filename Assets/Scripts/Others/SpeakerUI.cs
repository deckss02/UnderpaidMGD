using UnityEngine;
using UnityEngine.UI;

public class SpeakerUI : MonoBehaviour {
	public Image portrait;
	public Text fullName;
	public Text dialog;

	Character speaker;
	Line line;
	public Character Speaker {
		get { return speaker; }
		set {
			speaker = value;
			portrait.sprite = speaker.sprite[line.number];
			fullName.text = speaker.fullName;
		}
	}

	public string Dialog {
		set { dialog.text = value; }
	}

	public bool HasSpeaker() {
		return speaker != null;
	}

	public bool SpeakerIs(Character character) {
		return speaker == character;
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
}