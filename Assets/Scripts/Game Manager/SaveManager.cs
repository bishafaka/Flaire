using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// PROGRESSION SAVE CLASS
[Serializable]
public class SaveData
{
	public List<string> unlockedRecipes=new List<string>();
	public List<string> recievedLetters=new List<string>();
	public List<string> readLetters=new List<string>();
}

public class SaveManager : MonoBehaviour
{
	// SAVE DATA
	SaveData saveData=new SaveData();

	// PATH TO SAVE DATA
	string SavePath => Path.Combine(Application.persistentDataPath, "saveData.json");

	// EVENTS
	public event Action OnSaveDataChanged;

	// INIT
	void Awake()
	{
		LoadGame();
		Debug.Log(Application.persistentDataPath);
	}

// RECIPE PROGRESSION
	public bool IsRecipeUnlocked(Recipe recipe)
	{
		return recipe!=null && saveData.unlockedRecipes.Contains(recipe.RecipeID);
	}
	public void UnlockRecipe(Recipe recipe)
	{
		string id=recipe.RecipeID;
		if(saveData.unlockedRecipes.Contains(id))
			return;
		saveData.unlockedRecipes.Add(id);
		SaveGame();
		OnSaveDataChanged?.Invoke();
	}

// RECIPE COUNT
	public int UnlockedRecipeCount => saveData.unlockedRecipes.Count;

// LETTER PROGRESSION
	public bool HasRecievedLetter(Letter letter)
	{
		return letter!=null&&saveData.recievedLetters.Contains(letter.LetterID);
	}
	public void RecieveLetter(Letter letter)
	{
		string id=letter.LetterID;
		if(saveData.recievedLetters.Contains(id))
			return;
		saveData.recievedLetters.Add(id);
		SaveGame();
		OnSaveDataChanged?.Invoke();
	}
	public void MarkLetterAsRead(Letter letter)
	{
		string id=letter.LetterID;
		if(saveData.readLetters.Contains(id))
			return;
		saveData.readLetters.Add(id);
		SaveGame();
		OnSaveDataChanged?.Invoke();
	}

// LETTER COUNT
	public int RecievedLetterCount => saveData.recievedLetters.Count;
	public int ReadLetterCount => saveData.readLetters.Count;
	public int UnreadLetterCount => RecievedLetterCount-ReadLetterCount;

// SAVE GAME
	public void SaveGame()
	{
		try
		{
			string json=JsonUtility.ToJson(saveData, true);
			File.WriteAllText(SavePath, json);
			Debug.Log("Game saved!");
		}
		catch(Exception exception){ Debug.LogError($"Failed save: {exception.Message}"); }
	}

// LOAD GAME
	public void LoadGame()
	{
		if(!File.Exists(SavePath))
		{
			saveData=new SaveData();
			SaveGame();
			return;
		}
		try
		{
			string json=File.ReadAllText(SavePath);
			SaveData loadedData=JsonUtility.FromJson<SaveData>(json);
			saveData=loadedData??new SaveData();
			saveData.unlockedRecipes??=new List<string>();
			saveData.recievedLetters??=new List<string>();
			saveData.readLetters??=new List<string>();
			OnSaveDataChanged?.Invoke();
			Debug.Log("Game loaded!");
		}
		catch(Exception exception)
		{
			Debug.LogError($"Failed load: {exception.Message}");
			saveData=new SaveData();
		}
	}

// AUTO SAVE GAME ON PAUSE AND QUIT
	void OnApplicationPause(bool pause)
	{
		if(pause)
			SaveGame();
	}
	public void OnApplicationQuit()
	{
		SaveGame();
	}
}
