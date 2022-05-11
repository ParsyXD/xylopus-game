using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
	#region Variables
	public static bool cameraInputs = true;
	Vector2 mouseLook;
	Vector2 smoothV;

	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	GameObject character;
	#endregion

	void Start()
	{	
		character = (this.transform.parent.gameObject).transform.parent.gameObject;
		mouseLook.x = character.transform.eulerAngles.y;	
	}

	void Update()
	{
		if (!FPSController.playerInputs)
			return;

		if (cameraInputs)
			CameraMovement();
	}

	void CameraMovement()
	{
		var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp(mouseLook.y, -80f, 80f);
		transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
	}
}
