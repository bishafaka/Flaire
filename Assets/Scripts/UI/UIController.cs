using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
// UI REFERENCES
	[SerializeField] GameObject timePanel;
	[SerializeField] TextMeshProUGUI timeText;
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
}
