#if UNITY_EDITOR
using System;
# endif
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName="Recipe", menuName="Flaire/Recipe")]
public class Recipe : ScriptableObject
{
	public enum Category { Bread, Pastry, Cake, Cream }
	public enum Rarity { Common, Rare, Legendary, Seasonal }

// RECIPE DATA
	[SerializeField] string recipeID;
	[SerializeField] LocalizedString recipeName;
	[SerializeField] int recipePrice;
	[SerializeField] Sprite recipeIcon;
	[SerializeField] GameObject recipePrefab;
	[SerializeField] Category recipeCategory;
	[SerializeField] Rarity recipeRarity;

// GETTERS
	public string RecipeID => recipeID;
	public LocalizedString RecipeName => recipeName;
	public int RecipePrice => recipePrice;
	public Sprite RecipeIcon => recipeIcon;
	public GameObject RecipePrefab => recipePrefab;
	public Category RecipeCategory => recipeCategory;
	public Rarity RecipeRarity => recipeRarity;

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
