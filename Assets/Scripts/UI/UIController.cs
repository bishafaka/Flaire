using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
// UI REFERENCES
	[SerializeField] GameObject timePanel;
	[SerializeField] TextMeshProUGUI timeText;

// INIT & CLEANUP EVENT
	void Start()
	{
		GameManager.Instance.TimeManager.OnTimeChanged+=UpdateTime;
		UpdateTime();
	}
	void OnDestroy()
	{
		GameManager.Instance.TimeManager.OnTimeChanged-=UpdateTime;
	}

// UI TIME UPDATE
	void UpdateTime()
	{
		timeText.text=$"{GameManager.Instance.TimeManager.GetHour():00} : {GameManager.Instance.TimeManager.GetMinute():00}";
	}
}
