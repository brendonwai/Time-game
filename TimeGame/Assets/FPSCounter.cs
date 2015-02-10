using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	public  float updateInterval = 0.5F;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval

	public Text fpsText;
	
	void Start()
	{
		fpsText = GameObject.Find("FPSCounter").GetComponent<Text>();
		timeleft = updateInterval;  
	}
	
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			fpsText.text = format;
			
			if(fps < 30){
				fpsText.color = Color.yellow;
			}
			else { 
				if(fps < 10)
					fpsText.color = Color.red;
				else
				fpsText.color = Color.green;
			}
			//	DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
