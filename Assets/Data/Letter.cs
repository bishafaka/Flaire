using UnityEngine;

[CreateAssetMenu(fileName="Letter", menuName="Flaire/Letter")]
public class Letter : ScriptableObject
{
// LETTER DATA
	[SerializeField, TextArea] string letterContent;

// GETTERS
	public string LetterContent => letterContent;
}
