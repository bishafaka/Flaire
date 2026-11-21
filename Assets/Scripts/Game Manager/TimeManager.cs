using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
// WORKING HOURS
	const int INIT_DAY_HOUR=8;
	const int END_DAY_HOUR=20;

// DEBUG
	[SerializeField] float debugTimeSpeed=1f;

// TIME VARIABLES
	public event Action OnTimeChanged;
	public event Action OnDayChanged;
	int day=1;
	int hour=INIT_DAY_HOUR;
	int minute=0;
	float timer;
	bool finishedShift=false;

// UPDATE TIMER
	void Update()
	{
		if(finishedShift||GameManager.Instance.InputManager.GetPlayerState!=InputManager.PlayerState.Player)
			return;
		timer+=debugTimeSpeed*Time.deltaTime;
		while(timer>=1f)
		{
			timer-=1f;
			minute++;
			if(minute>=60)
			{
				minute=0;
				hour++;
			}
			OnTimeChanged?.Invoke();
			if(hour>=END_DAY_HOUR)
			{
				finishedShift=true;
				break;
			}
		}
	}

// RESET TIMER
	public void SetNewDay()
	{
		hour=INIT_DAY_HOUR;
		minute=0;
		timer=0;
		day++;
		finishedShift=false;
		OnDayChanged?.Invoke();
		Debug.Log("New day!");
	}

// GETTERS
	public int GetDay() { return day; }
	public int GetHour() { return hour; }
	public int GetMinute() { return minute; }
	public bool IsShiftFinished() { return finishedShift; }
}
