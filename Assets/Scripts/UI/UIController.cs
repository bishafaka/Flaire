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
	InputManager inputManager;

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

// EVENT SUBSCRIPTION
	void OnEnable()
	{
		inputManager=GameManager.Instance.InputManager;
		if(inputManager!=null)
		{
			inputManager.pausePressed+=Pause;
		}
	}
	void OnDisable()
	{
		if(inputManager!=null)
		{
			inputManager.pausePressed-=Pause;
		}
	}
// UI TIME
	void UpdateTime()
	{
		timeText.text=$"{timeManager.GetHour():00} : {timeManager.GetMinute():00}";
	}

// PAUSE PRESSED
	void Pause()
	{
		if(letterPanel.activeSelf)
		{
			HideLetter();
			return;
		}
		if(mailboxPanel.activeSelf)
		{
			HideMailbox();
			return;
		}
	}

// UI MAILBOX
	public void ShowMailbox()
	{
		mailboxPanel.SetActive(true);
		timePanel.SetActive(false);
		inputManager.SetPlayerState(InputManager.PlayerState.UI);
	}
	public void HideMailbox()
	{
		mailboxPanel.SetActive(false);
		timePanel.SetActive(true);
		inputManager.SetPlayerState(InputManager.PlayerState.Player);
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
