using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
// SINGLETON
	public static GameManager Instance { get; private set; }

// MANAGER REFERENCES
	public InputManager InputManager { get; private set; }
	public TimeManager TimeManager { get; private set; }
	public SaveManager SaveManager { get; private set; }

// GAME VARIABLES
	const int DEFAULT_CHANCE=25;
	const int MAX_CHANCE=100;
	int currentChance=DEFAULT_CHANCE;

// INIT SINGLETON & MANAGER REFERENCES
	void Awake()
	{
		if(Instance!=null && Instance!=this)
		{
			Destroy(gameObject);
			return;
		}
		Instance=this;
		DontDestroyOnLoad(gameObject);

	// GET MANAGERS REFERENCES
		InputManager=GetComponent<InputManager>();
		TimeManager=GetComponent<TimeManager>();
		SaveManager=GetComponent<SaveManager>();
	}

// SUBSCRIBE TO EVENTS
	void OnEnable()
	{
		if(Instance!=this || TimeManager==null)
			return;
		TimeManager.OnDayChanged-=LetterGamble;
		TimeManager.OnDayChanged+=LetterGamble;
	}
	void OnDisable()
	{
		if(TimeManager==null)
			return;
		TimeManager.OnDayChanged-=LetterGamble;
	}

// DAY UPDATE NEW MAIL RECIEVE
	void LetterGamble()
	{
		if(Instance!=this || SaveManager==null || TimeManager==null)
			return;
		List<Letter> availableLetters=SaveManager.allLetters.FindAll(letter => !SaveManager.HasRecievedLetter(letter));
		if(availableLetters.Count==0)
			return;
		int random=Random.Range(0, MAX_CHANCE);
		if(random<currentChance)
		{
		// IF MAIL IS RECIEVED, RESETS CHANCE TO THE DEFAULT
			int index=Random.Range(0, availableLetters.Count);
			SaveManager.RecieveLetter(availableLetters[index]);
			currentChance=DEFAULT_CHANCE;
			Debug.Log("New mail!");
		}
		else
		{
		// IF MAIL IS NOT RECIEVED, INCREASES THE CHANCE FOR NEXT DAY
			currentChance+=DEFAULT_CHANCE;
			currentChance=Mathf.Min(currentChance, MAX_CHANCE);
			Debug.Log("No mail...");
		}
		Debug.Log($"Chance: {currentChance}");
	}
}
