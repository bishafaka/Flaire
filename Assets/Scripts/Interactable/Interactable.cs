using UnityEngine;

public class Interactable : MonoBehaviour
{
// INTERACTABLE CATEGORY
	public enum Category { Mailbox, Door }
	[SerializeField] Category type;

// ON INTERACT VIRTUAL METHOD
	public virtual void Interact()
	{
	}

// GETTERS
	public Category GetCategory() { return type; }
}
