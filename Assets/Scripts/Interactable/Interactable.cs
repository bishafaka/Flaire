using UnityEngine;

public class Interactable : MonoBehaviour
{
// INTERACTABLE CATEGORY
	public enum Category { Mailbox }
	[SerializeField] Category type;

// GETTERS
	public Category GetCategory() { return type; }
}
