using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {
	public float sensitivity = 0.2f;
	public float maxAmount = 0.08f;
	public float speed = 6;

	private Vector3 initialPosition;

	void Start () {
		initialPosition = transform.localPosition;
	}
	
	void Update () {
		float moveX = -Input.GetAxis("Mouse X") * sensitivity;
		float moveY = -Input.GetAxis("Mouse Y") * sensitivity;
		moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
		moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

		Vector3 finalPosition = new Vector3(moveX, moveY, 0);
		transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * speed);
	}
}
