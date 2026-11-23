using UnityEngine;

public class RecipeCard : MonoBehaviour
{
	[SerializeField] UIController uiController;
	Recipe recipe;
	public Recipe SetRecipe(Recipe newRecipe) => recipe=newRecipe;
	public void SelectCard()
	{
		GameManager.Instance.SaveManager.UnlockRecipe(recipe);
		uiController.HideRecipeCards();
	}
}
