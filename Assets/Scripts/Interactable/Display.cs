using UnityEngine;

public class Display : Interactable
{
	[Header("Display Slots")]
	[SerializeField] DisplaySlot[] slots;
	public DisplaySlot[] Slots => slots;
}
