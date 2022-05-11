using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightVisualizer : MonoBehaviour
{
	public GameObject canvasVisualizer;
	public Vector3 canvasOffset;

	public bool weightInLbs;
	public bool showVelocity;

	private float weight;
	private string weightStr;
	private string objectWeight;
	private Rigidbody rb;

	private GameObject character;
	private GameObject worldCanvas;
	private Text canvasText;
	private Camera cam;

    void Start()
    {
		character = GameObject.Find("Character");
		cam = Camera.main;

		if (GetComponent<Rigidbody>())
		{
			rb = GetComponent<Rigidbody>();
			weight = rb.mass * 9.81f;
			objectWeight = weight.ToString();
		}

		worldCanvas = Instantiate(canvasVisualizer, transform.position + canvasOffset, Quaternion.identity);
		worldCanvas.GetComponent<Canvas>().worldCamera = cam;
		worldCanvas.transform.SetParent(this.transform);
		canvasText = worldCanvas.transform.GetChild(0).GetComponent<Text>();
		worldCanvas.transform.GetChild(0).GetComponent<Text>().text = "Weight: " + objectWeight;	
	}

    void LateUpdate()
    {
		UpdateUI();
    }

	void UpdateUI()
	{
		if(worldCanvas != null && rb)
		{
			weight = rb.mass * 9.81f;
			if (weightInLbs)
			{
				weight = ConvertToLb(weight);
				weightStr = "lbs";
			}
			else
			{
				weightStr = "kgs";
			}
			
			objectWeight = weight.ToString();
			canvasText.text = string.Format("Weight: {0} {1}", weight.ToString("F2"), weightStr);

			if(showVelocity)
			{
				float velocity = rb.velocity.magnitude;
				canvasText.text = string.Format("Weight: {0} {1}", weight.ToString("F2"), weightStr) + "\n" + string.Format("Velocity: {0} u/s", velocity.ToString("F2"));
			}
				
			worldCanvas.transform.rotation = Quaternion.LookRotation(worldCanvas.transform.position - cam.transform.position);
		}
			
			
		worldCanvas.transform.position = transform.position + canvasOffset;
	}

	float ConvertToLb(float kg)
	{
		float lbs = kg * 2.205f;
		return lbs;
	}
}
