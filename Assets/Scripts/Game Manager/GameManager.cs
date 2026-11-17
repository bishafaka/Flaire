using UnityEngine;

public class GameManager : MonoBehaviour
{
	// SINGLETON
	public static GameManager Instance { get; private set; }

	// MANAGER REFERENCES
	public InputManager InputManager { get; private set; }

	// INIT SINGLETON & MANAGER REFERENCES
	void Awake()
	{
		if(Instance==null)
		{
			Instance=this;
			DontDestroyOnLoad(this);
			if(InputManager==null)
				InputManager=GetComponent<InputManager>();
		}
		else
			Destroy(gameObject);
	}
}
