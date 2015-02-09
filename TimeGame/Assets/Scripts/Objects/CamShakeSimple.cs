using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour 
{
	public Camera mainCamera;

	Vector3 originalCameraPosition;
	
	float shakeAmt = 0;
	

	
	void CamShakeOnDamage(float damage) 
	{
		originalCameraPosition = mainCamera.transform.position;
		shakeAmt = damage * .0015f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
		
	}
	
	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.x+= quakeAmt; // can also add to x and/or z
			pp.y+=quakeAmt;
			mainCamera.transform.position = pp;
		}
	}
	
	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}
	
}