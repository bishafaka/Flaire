using UnityEngine;

public class Interactable : MonoBehaviour
{
// INTERACTABLE CATEGORY
	public enum Category { Mailbox, Door, Storage, Counter, Oven, Display }
	[SerializeField] Category type;

// ON INTERACT VIRTUAL METHOD
	public virtual void Interact()
	{
	}

// GETTERS
	public Category GetCategory() { return type; }
}
