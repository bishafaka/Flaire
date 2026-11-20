using TMPro;
using UnityEngine;

public class LetterButton : MonoBehaviour
{
// LETTER DATA
	public Letter letter;

// REFERENCES
	[SerializeField] TextMeshProUGUI letterSender;

// INIT SENDER TEXT
	void Awake()
	{
		letter.LetterSender.StringChanged+=InitText;
	}
	void InitText(string value)
	{
		letterSender.text=value;
	}
}
