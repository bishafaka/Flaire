using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName="Letter", menuName="Flaire/Letter")]
public class Letter : ScriptableObject
{
// LETTER DATA
	[SerializeField] LocalizedString letterSender;
	[SerializeField] LocalizedString letterContent;
	[SerializeField] Recipe recipe;

// GETTERS
	public LocalizedString LetterSender => letterSender;

	public LocalizedString LetterContent => letterContent;
}
