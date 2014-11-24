using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	public GUIStyle myStyle;
	void OnGUI()
	{
		const int buttonWidth = 94;
		const int buttonHeight = 50;

		const int button2Width = 124;
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2* Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);

		Rect buttonRect2 = new Rect(
			Screen.width / 2 - (button2Width / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2)+(Screen.height/6),
			button2Width,
			buttonHeight
			);
		
		// Draw a button to start the game
		if(GUI.Button(buttonRect,"Restart",myStyle))
		{
			// On Click, load the first level.
			Application.LoadLevel("TG - Level 0");
		}
		if(GUI.Button (buttonRect2,"Main Menu",myStyle))
		{
			Application.LoadLevel ("MainMenu");
		}

	}
}