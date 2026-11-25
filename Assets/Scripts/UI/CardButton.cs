using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
// RECIPE DATA
	Recipe recipe;
	UIController uiController;

// REFERENCES
	[SerializeField] TextMeshProUGUI recipeName;
	[SerializeField] Image recipeImage;

// INIT
	public void Init(Recipe newRecipe, UIController newUIController)
	{
		recipe=newRecipe;
		uiController=newUIController;
		recipeImage.sprite=recipe.RecipeIconFront;
		recipe.RecipeName.StringChanged+=InitText;
	}

// SUBSCRIBE EVENTS
	void OnDisable()
	{
		recipe.RecipeName.StringChanged-=InitText;
	}
	void InitText(string value)
	{
		recipeName.text=value;
	}

// PICK UP RECIPE
	public void PickupRecipe()
	{
		PlayerController.Instance.PickUpRecipe(recipe);
		uiController.HideRecipes();
	}
}
