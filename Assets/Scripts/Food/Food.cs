using UnityEngine;

public class Food : MonoBehaviour
{
	public enum Stage { Ingredients, Raw, Cooked }
	[Header("Mesh")]
	[SerializeField] GameObject ingredientsMesh;
	[SerializeField] GameObject rawMesh;
	[SerializeField] GameObject cookedMesh;
	[Header("Recipe Data")]
	[SerializeField] Recipe recipe;
	[SerializeField] Stage stage;
	public Recipe Recipe => recipe;
	public Stage CurrentStage => stage;

// INIT ITEM
	public void Init(Recipe newRecipe, Stage newStage)
	{
		recipe=newRecipe;
		SetStage(newStage);
	}

// CHANGE VISUAL STAGE
	public void SetStage(Stage newStage)
	{
		stage=newStage;
		ingredientsMesh.SetActive(stage==Stage.Ingredients);
		rawMesh.SetActive(stage==Stage.Raw);
		cookedMesh.SetActive(stage==Stage.Cooked);
	}
}
