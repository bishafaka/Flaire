using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;

public class UIController : MonoBehaviour
{
// UI REFERENCES
	[Header("CAMERAS")]
	[SerializeField] CinemachineCamera sceneCam;
	[SerializeField] CinemachineCamera mailCam;
	[Header("TIME")]
	[SerializeField] GameObject timePanel;
	[SerializeField] TextMeshProUGUI timeText;
	[SerializeField] GameObject skipDayPanel;
	[Header("MAIL")]
	[SerializeField] GameObject mailboxPanel;
	public LetterButton[] mailBoxLetters;
	[SerializeField] GameObject letterPanel;
	[SerializeField] TextMeshProUGUI letterText;
	[SerializeField] GameObject letterPresent;
	[SerializeField] GameObject recipeCardPanel;
	[SerializeField] GameObject recipeCardImage;
	[Header("LOADING")]
	[SerializeField] GameObject loadingPanel;
	[SerializeField] Slider progressSlider;
	[SerializeField] TMP_Text progressText;
	TimeManager timeManager;
	InputManager inputManager;
	Letter currentLetter;

// INIT & CLEANUP EVENT
	void Start()
	{
		SetCameraPriority(sceneCam);
		timeManager=GameManager.Instance.TimeManager;
		timeManager.OnTimeChanged+=UpdateTime;
		timeManager.OnDayChanged+=UpdateDay;
		UpdateTime();
	}
	void OnDestroy()
	{
		timeManager.OnTimeChanged-=UpdateTime;
		timeManager.OnDayChanged-=UpdateDay;
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
	void UpdateDay()
	{
		ChangeScene(0);
	}

// PAUSE PRESSED
	void Pause()
	{
		if(recipeCardPanel.activeSelf)
		{
			HideRecipeCard();
			return;
		}
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
		if(skipDayPanel.activeSelf)
		{
			HideSkipPanel();
			return;
		}
	}

// UI MAILBOX
	public void ShowMailbox()
	{
		SetCameraPriority(mailCam);
	// CHECKS THE RECIEVED LETTERS FROM SAVE DATA AND SHOWS THEM
		SaveManager save=GameManager.Instance.SaveManager;
		foreach(LetterButton letter in mailBoxLetters)
		{
			letter.gameObject.SetActive(false);
			if(save.HasRecievedLetter(letter.letter))
			{
				letter.gameObject.transform.SetAsFirstSibling();
				letter.gameObject.SetActive(true);
			}
		}
		mailboxPanel.SetActive(true);
		timePanel.SetActive(false);
		inputManager.SetPlayerState(InputManager.PlayerState.UI);
	}
	public void HideMailbox()
	{
		SetCameraPriority(sceneCam);
		mailboxPanel.SetActive(false);
		timePanel.SetActive(true);
		inputManager.SetPlayerState(InputManager.PlayerState.Player);
	}

// UI LETTER
	public void ShowLetter(Letter letter)
	{
		mailboxPanel.SetActive(false);
		letterPanel.SetActive(true);
		currentLetter=letter;
		currentLetter.LetterContent.StringChanged+=UpdateText;
		if(currentLetter.Recipe==null)
			letterPresent.SetActive(false);
		else
			letterPresent.SetActive(true);
	}
	void UpdateText(string value)
	{
		letterText.text=value;
	}
	public void HideLetter(bool showMailbox=true)
	{
		if(showMailbox)
			mailboxPanel.SetActive(true);
		letterPanel.SetActive(false);
		currentLetter.LetterContent.StringChanged-=UpdateText;
	}
	public void SelectPresent()
	{
		Recipe presentRecipe=currentLetter.Recipe;
		ShowRecipeCard(presentRecipe);
	}

// UI RECIPE CARD
	void ShowRecipeCard(Recipe recipe)
	{
		HideLetter(false);
		recipeCardPanel.SetActive(true);
		recipeCardImage.GetComponent<UIDraggableCard>().InitCard(recipe);
		GameManager.Instance.SaveManager.UnlockRecipe(recipe);
	}
	public void HideRecipeCard()
	{
		recipeCardPanel.SetActive(false);
		ShowLetter(currentLetter);
	}

// UI SKIP PANEL
	public void ShowSkipPanel()
	{
		skipDayPanel.SetActive(true);
		inputManager.SetPlayerState(InputManager.PlayerState.UI);
	}
	public void HideSkipPanel(bool fastForward=false)
	{
		skipDayPanel.SetActive(false);
		if(fastForward)
		{
			timeManager.FastForward();
			inputManager.SetPlayerState(InputManager.PlayerState.Cinematic);
		}
		else
			inputManager.SetPlayerState(InputManager.PlayerState.Player);
	}

// UI LOADING PANEL
	public void ChangeScene(int sceneIndex)
	{
		StartCoroutine(LoadSceneAsync(sceneIndex));
	}
	IEnumerator LoadSceneAsync(int sceneIndex)
	{
		loadingPanel.SetActive(true);
		AsyncOperation operation=SceneManager.LoadSceneAsync(sceneIndex);
		while(!operation.isDone)
		{
			float progress=Mathf.Clamp01(operation.progress/0.9f);
			progressSlider.value=progress;
			progressText.text=$"{Mathf.RoundToInt(progress*100)}%";
			yield return new WaitForSeconds(1f);
		}
	}

// CAMERA BEHAVIOUR
	void SetCameraPriority(CinemachineCamera cam)
	{
		sceneCam.Priority=0;
		mailCam.Priority=0;
		cam.Priority=10;
	}
}
