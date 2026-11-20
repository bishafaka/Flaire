using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
// UI REFERENCES
	[SerializeField] GameObject timePanel;
	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] GameObject mailboxPanel;
	[SerializeField] LetterButton[] mailBoxLetters;
	[SerializeField] GameObject letterPanel;
	[SerializeField] TextMeshProUGUI letterText;
	TimeManager timeManager;

// INIT & CLEANUP EVENT
	void Start()
	{
		timeManager=GameManager.Instance.TimeManager;
		timeManager.OnTimeChanged+=UpdateTime;
		UpdateTime();
	}
	void OnDestroy()
	{
		timeManager.OnTimeChanged-=UpdateTime;
	}

// UI TIME
	void UpdateTime()
	{
		timeText.text=$"{timeManager.GetHour():00} : {timeManager.GetMinute():00}";
	}

// UI MAILBOX
	public void ToggleMailbox()
	{
		if(mailboxPanel.activeSelf)
		{
			GameManager.Instance.InputManager.SetPlayerState(InputManager.PlayerState.Player);
			mailboxPanel.SetActive(false);
			timePanel.SetActive(true);
		}
		else
		{
			GameManager.Instance.InputManager.SetPlayerState(InputManager.PlayerState.UI);
			mailboxPanel.SetActive(true);
			timePanel.SetActive(false);
		}
	}

// UI LETTER
	public void ShowLetter(Letter letter)
	{
		mailboxPanel.SetActive(false);
		letterPanel.SetActive(true);
		letter.LetterContent.StringChanged+=UpdateText;
	}
	void UpdateText(string value)
	{
		letterText.text=value;
	}
	public void HideLetter()
	{
		mailboxPanel.SetActive(true);
		letterPanel.SetActive(false);
	}
}
