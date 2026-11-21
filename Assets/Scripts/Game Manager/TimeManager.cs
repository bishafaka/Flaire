using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
// WORKING HOURS / MULTIPLIERS
	const int INIT_DAY_HOUR=8;
	const int END_DAY_HOUR=20;
	const int FAST_FORWARD_MULTIPLIER=250;

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
	bool isFastForward=false;
	int timeMultiplier=1;

// REFERENCES
	InputManager inputManager;

// INIT
	void Awake()
	{
		inputManager=GameManager.Instance.InputManager;
	}

// UPDATE TIMER
	void Update()
	{
		if(finishedShift || (inputManager.GetPlayerState!=InputManager.PlayerState.Player && !isFastForward))
			return;
		timer+=debugTimeSpeed*Time.deltaTime*timeMultiplier;
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
				isFastForward=false;
				finishedShift=true;
				timeMultiplier=1;
				inputManager.SetPlayerState(InputManager.PlayerState.Player);
				break;
			}
		}
	}

// FAST FORWARD
	public void FastForward()
	{
		if(isFastForward)
			return;
		timeMultiplier=FAST_FORWARD_MULTIPLIER;
		isFastForward=true;
		inputManager.SetPlayerState(InputManager.PlayerState.Cinematic);
	}

// NEW DAY
	public void SetNewDay()
	{
		hour=INIT_DAY_HOUR;
		minute=0;
		timer=0;
		finishedShift=false;
		isFastForward=false;
		timeMultiplier=1;
		inputManager.SetPlayerState(InputManager.PlayerState.Player);
		day++;
		OnDayChanged?.Invoke();
		OnTimeChanged?.Invoke();
		Debug.Log("New day!");
	}

// GETTERS
	public int GetDay() { return day; }
	public int GetHour() { return hour; }
	public int GetMinute() { return minute; }
	public bool IsShiftFinished() { return finishedShift; }
}
