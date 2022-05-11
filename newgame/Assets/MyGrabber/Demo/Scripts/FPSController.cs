using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
	#region Variables
	public static bool playerInputs = true;

	public float runSpeed;
	public float walkSpeed;
	float actualSpeed;
	public CharacterController controller;

	public float jumpForce;
	public float gravity;
	float velocityY;
	public Transform groundCheck;
	public float checkRadius;
	public bool onGround;
	public LayerMask groundLayer;
	public bool isRunning;
    #endregion

	void Start()
	{
		playerInputs = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		walkSpeed = 0.33f * runSpeed;
		actualSpeed = walkSpeed;
	}
    
    void Update()
    {
		if(playerInputs)
		{
			Move();
			Jump();
		}	
	}

	void Move()
	{
		float translation = Input.GetAxis("Vertical") * actualSpeed;
		float straffe = Input.GetAxis("Horizontal") * actualSpeed;
		if (translation != 0f || straffe != 0f)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				actualSpeed = runSpeed;
				isRunning = true;
			}
			else
			{
				actualSpeed = walkSpeed;
				isRunning = false;
			}
		}
		else
		{
			actualSpeed = walkSpeed;
			isRunning = false;
		}
		velocityY += Time.deltaTime * gravity;
		Vector3 movePos = transform.TransformDirection(new Vector3(straffe, velocityY, translation));
		controller.Move(movePos * Time.deltaTime);
	}

	void Jump()
	{
		onGround = Physics.CheckBox(groundCheck.position, new Vector3(0.5f, checkRadius, 0.5f), groundCheck.rotation, groundLayer);
		if (onGround && velocityY < 0f)
		{
			velocityY = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{		
			if (onGround)
			{
				float jumpVel = Mathf.Sqrt(-2 * gravity * jumpForce);
				velocityY = jumpVel;
			}
		}	
	}
}
