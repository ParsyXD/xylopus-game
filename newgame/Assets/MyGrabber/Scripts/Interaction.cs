using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGrabber3D
{
	[RequireComponent(typeof(MyGrabber))]
	public class Interaction : MonoBehaviour, IMyGrabberInputHandler
	{
		public Image pointer;
		public Image pointer_Circle;
		public LayerMask InteractionLayer;
		[SerializeField] private float distanceToInteract = 3.5f;
		public bool holdForGrab = true;
		public KeyCode grabInput = KeyCode.Mouse1;
		public KeyCode throwInput = KeyCode.Mouse2;
		public KeyCode rotationKey = KeyCode.R;
		private Color whiteIdle = new Color(1, 1, 1, 0.25f);
		private Color whiteInteract = new Color(1, 1, 1, 0f);//1f to 0f
		private Vector3 baseScale = new Vector3(0.1f, 0.1f, 1f);
		private Vector3 interactScale = new Vector3(0.12f, 0.12f, 1f);
		private Vector3 circleBase = new Vector3(0f, 0f, 0f);
		private Vector3 circleInteract = new Vector3(0.14f, 0.14f, 1f);
		private Color circleInteractColor = new Color(1f, 1f, 1f, 1f);
		private MyGrabber myGrabber;
		private bool justGrabbed = false;

		public bool cameraInputs
		{
			get { return CameraController.cameraInputs; }
			set { CameraController.cameraInputs = value; }
		}

		private void Awake()
        {
			myGrabber = GetComponent<MyGrabber>();
		}

		void Update()
		{
			if (!myGrabber.enabled)
				return;

			bool pointingAtObject = false;
			if (myGrabber.grabbedObj == null)
			{
				Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;

				if (Physics.Raycast(rayOrigin, out hitInfo, distanceToInteract, InteractionLayer))//Is pointing at interactable object
				{
					Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.red);

					// Check for grab input
					if (Input.GetKeyDown(grabInput))
					{
						myGrabber.GrabObject(hitInfo.collider.gameObject);
						justGrabbed = true;
					}
					else
						pointingAtObject = true;
				}
			}
			else
			{
				if (Input.GetKey(rotationKey))
				{
					cameraInputs = false;
					float mouseX = Input.GetAxis("Mouse X");
					float mouseY = Input.GetAxis("Mouse Y");
					myGrabber.RotateObject(mouseX, mouseY);
				}
				else if (Input.GetKeyUp(rotationKey))
				{
					cameraInputs = true;
				}

				// Check for throw input
				if (Input.GetKeyDown(throwInput))
					myGrabber.ThrowObject();

				// Check for release input
				if ((Input.GetKeyUp(grabInput) && holdForGrab || Input.GetKeyDown(grabInput) && !holdForGrab && !justGrabbed))
					myGrabber.ReleaseObject();
			}

			// Handle HUD feedback
			if (pointingAtObject)
            {
				if (pointer != null)
				{
					pointer.color = Color.Lerp(pointer.color, whiteInteract, 7f * Time.deltaTime);
					pointer.transform.localScale = Vector3.Slerp(pointer.transform.localScale, interactScale, 7f * Time.deltaTime);
				}
				if (pointer_Circle != null)
				{
					pointer_Circle.color = Color.Lerp(pointer_Circle.color, circleInteractColor, 7f * Time.deltaTime);
					pointer_Circle.transform.localScale = Vector3.Slerp(pointer_Circle.transform.localScale, circleInteract, 7f * Time.deltaTime);
				}
			}
			else
			{
				if (pointer != null)
				{
					if (pointer.color != whiteIdle)
					{
						pointer.color = Color.Lerp(pointer.color, whiteIdle, 10f * Time.deltaTime);
						pointer.transform.localScale = Vector3.Slerp(pointer.transform.localScale, baseScale, 10f * Time.deltaTime);
					}
				}
				if (pointer_Circle != null)
				{
					if (pointer.color != whiteIdle)
					{
						pointer_Circle.color = Color.Lerp(pointer_Circle.color, whiteIdle, 7f * Time.deltaTime);
						pointer_Circle.transform.localScale = Vector3.Slerp(pointer_Circle.transform.localScale, circleBase, 7f * Time.deltaTime);
					}
				}
			}

			justGrabbed = false;
		}
	}
}