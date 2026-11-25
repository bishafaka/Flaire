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

// CACHED LISTS
	List<Recipe> unlockedRecipes=new List<Recipe>();
	List<Letter> receivedLetters=new List<Letter>();
	List<Letter> readLetters=new List<Letter>();

// GETTERS
	public List<Recipe> UnlockedRecipes => unlockedRecipes;
	public List<Letter> ReceivedLetters => receivedLetters;
	public List<Letter> ReadLetters => readLetters;

// PATH TO SAVE DATA
	string SavePath => Path.Combine(Application.persistentDataPath, "saveData.json");

// ALL DATA
	public List<Letter> allLetters=new List<Letter>();
	public List<Recipe> allRecipes=new List<Recipe>();

// INIT
	void Awake()
	{
		LoadGame();
		Debug.Log(Application.persistentDataPath);
	}

// REFRESH LISTS
	void RefreshProgressionLists()
	{
		unlockedRecipes=allRecipes.FindAll(IsRecipeUnlocked);
		receivedLetters=allLetters.FindAll(HasRecievedLetter);
		readLetters=allLetters.FindAll(HasReadLetter);
	}

// RECIPE PROGRESSION
	public bool IsRecipeUnlocked(Recipe recipe)
	{
		return recipe!=null && saveData.unlockedRecipes.Contains(recipe.RecipeID);
	}
	public void UnlockRecipe(Recipe recipe)
	{
		if(recipe==null)
			return;
		string id=recipe.RecipeID;
		if(saveData.unlockedRecipes.Contains(id))
			return;
		saveData.unlockedRecipes.Add(id);
		SaveGame();
		RefreshProgressionLists();
	}

// RECIPE COUNT
	public int UnlockedRecipeCount => unlockedRecipes.Count;

// LETTER PROGRESSION
	public bool HasRecievedLetter(Letter letter)
	{
		return letter!=null && saveData.recievedLetters.Contains(letter.LetterID);
	}
	public void RecieveLetter(Letter letter)
	{
		if(letter==null)
			return;
		string id=letter.LetterID;
		if(saveData.recievedLetters.Contains(id))
			return;
		saveData.recievedLetters.Add(id);
		SaveGame();
		RefreshProgressionLists();
	}
	public bool HasReadLetter(Letter letter)
	{
		return letter!=null && saveData.readLetters.Contains(letter.LetterID);
	}
	public void MarkLetterAsRead(Letter letter)
	{
		if(!HasRecievedLetter(letter))
			return;
		string id=letter.LetterID;
		if(saveData.readLetters.Contains(id))
			return;
		saveData.readLetters.Add(id);
		SaveGame();
		RefreshProgressionLists();
	}

// LETTER COUNT
	public int RecievedLetterCount => receivedLetters.Count;
	public int ReadLetterCount => readLetters.Count;
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
		catch(Exception exception) { Debug.LogError($"Failed save: {exception.Message}"); }
	}

	// LOAD GAME
	public void LoadGame()
	{
		if(!File.Exists(SavePath))
		{
			saveData=new SaveData();
			SaveGame();
			RefreshProgressionLists();
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
			RefreshProgressionLists();
			Debug.Log("Game loaded!");
		}
		catch(Exception exception)
		{
			Debug.LogError($"Failed load: {exception.Message}");
			saveData=new SaveData();
			RefreshProgressionLists();
		}
	}

// AUTO SAVE GAME ON PAUSE AND QUIT
	void OnApplicationPause(bool pause)
	{
		if(pause)
			SaveGame();
	}
	void OnApplicationQuit()
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
