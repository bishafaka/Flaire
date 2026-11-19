using UnityEngine;

public class Interactable : MonoBehaviour
{
// INTERACTABLE CATEGORY
	public enum Category { Mailbox, Door }
	[SerializeField] Category type;

// GETTERS
	public Category GetCategory() { return type; }
}
