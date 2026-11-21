#if UNITY_EDITOR
using UnityEditor;
#endif
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

// ALL DATA 
	public List<Letter> allLetters=new List<Letter>();
	public List<Recipe> allRecipes=new List<Recipe>();

// INIT
	void Awake()
	{
		LoadGame();
	// REMOVES LETTERS ALREADY SEEN FROM SAVE DATA
		for(int i=allLetters.Count-1; i>=0; i--)
		{
			if(HasRecievedLetter(allLetters[i]))
				allLetters.RemoveAt(i);
		}
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

// REFRESH ALL RECIPES AND LETTERS LISTS
#if UNITY_EDITOR
	[ContextMenu("Collect All Data")]
	void CollectAllData()
	{
		allLetters.Clear();
		allRecipes.Clear();
		string[] letterGuids=AssetDatabase.FindAssets("t:Letter");
		foreach(string guid in letterGuids)
		{
			string path=AssetDatabase.GUIDToAssetPath(guid);
			Letter letter=AssetDatabase.LoadAssetAtPath<Letter>(path);
			if(letter!=null)
				allLetters.Add(letter);
		}
		string[] recipeGuids=AssetDatabase.FindAssets("t:Recipe");
		foreach(string guid in recipeGuids)
		{
			string path=AssetDatabase.GUIDToAssetPath(guid);
			Recipe recipe=AssetDatabase.LoadAssetAtPath<Recipe>(path);
			if(recipe!=null)
				allRecipes.Add(recipe);
		}
		EditorUtility.SetDirty(this);
	}
#endif
}
