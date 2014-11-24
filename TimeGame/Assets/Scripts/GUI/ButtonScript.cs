using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	public GUIStyle myStyle;
	void OnGUI()
	{
		const int buttonWidth = 94;
		const int buttonHeight = 50;
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);
		
		// Draw a button to start the game
		if(GUI.Button(buttonRect,"Restart",myStyle))
		{
			GUI.contentColor=Color.red;
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("TG - Level 0");
		}
	}
}