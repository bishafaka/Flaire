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
		if(Instance!=null&&Instance!=this)
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
		if(TimeManager!=null)
			TimeManager.OnDayChanged+=LetterGamble;
	}
	void OnDisable()
	{
		if(TimeManager!=null)
			TimeManager.OnDayChanged-=LetterGamble;
	}

// DAY UPDATE NEW MAIL RECIEVE
	void LetterGamble()
	{
		if(SaveManager.allLetters.Count<=0)
			return;
		int random=Random.Range(0, MAX_CHANCE);
		if(random<currentChance)
		{
		// IF MAIL IS RECIEVED, RESETS CHANCE TO THE DEFAULT
			int index=Random.Range(0, SaveManager.allLetters.Count);
			SaveManager.RecieveLetter(SaveManager.allLetters[index]);
			SaveManager.allLetters.RemoveAt(index);
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
