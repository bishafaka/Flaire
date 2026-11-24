#if UNITY_EDITOR
using System;
# endif
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName="Recipe", menuName="Flaire/Recipe")]
public class Recipe : ScriptableObject
{
	public enum Category { Bread, Pastry, Cake, Cream }

// RECIPE DATA
	[SerializeField] string recipeID;
	[SerializeField] LocalizedString recipeName;
	[SerializeField] int recipePrice;
	[SerializeField] Sprite recipeIconFront;
	[SerializeField] Sprite recipeIconBack;
	[SerializeField] GameObject recipePrefab;
	[SerializeField] Category recipeCategory;

// GETTERS
	public string RecipeID => recipeID;
	public LocalizedString RecipeName => recipeName;
	public int RecipePrice => recipePrice;
	public Sprite RecipeIconFront => recipeIconFront;
	public Sprite RecipeIconBack => recipeIconBack;
	public GameObject RecipePrefab => recipePrefab;
	public Category RecipeCategory => recipeCategory;

// ID GENERATOR
#if UNITY_EDITOR
	void OnValidate()
	{
		if(string.IsNullOrEmpty(recipeID))
		{
			recipeID=Guid.NewGuid().ToString("N").Substring(0, 10);
			UnityEditor.EditorUtility.SetDirty(this);
		}
	}
#endif
}
