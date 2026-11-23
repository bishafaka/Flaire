#if UNITY_EDITOR
using System;
#endif
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName="Letter", menuName="Flaire/Letter")]
public class Letter : ScriptableObject
{
// LETTER DATA
	[SerializeField] string letterID;
	[SerializeField] LocalizedString letterSender;
	[SerializeField] LocalizedString letterContent;
	[SerializeField] Recipe[] recipe;

// GETTERS
	public string LetterID => letterID;
	public LocalizedString LetterSender => letterSender;
	public LocalizedString LetterContent => letterContent;
	public Recipe[] Recipe => recipe;

// ID GENERATOR
#if UNITY_EDITOR
	void OnValidate()
	{
		if(string.IsNullOrEmpty(letterID))
		{
			letterID=Guid.NewGuid().ToString("N").Substring(0, 10);
			UnityEditor.EditorUtility.SetDirty(this);
		}
	}
#endif
}
