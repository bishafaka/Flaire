using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
// UI REFERENCES
	[SerializeField] GameObject timePanel;
	[SerializeField] TextMeshProUGUI timeText;
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

// UI TIME UPDATE
	void UpdateTime()
	{
		timeText.text=$"{timeManager.GetHour():00} : {timeManager.GetMinute():00}";
	}

// UI LETTER UPDATE
	public void ShowLetter(Letter letter)
	{
		GameManager.Instance.InputManager.SetPlayerState(InputManager.PlayerState.UI);
		letterPanel.SetActive(true);
		letter.LetterContent.StringChanged+=UpdateText;
	}
	void UpdateText(string value)
	{
		letterText.text=value;
	}
}
