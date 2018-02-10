using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {
	public float sensitivity;
	public float speed;

	private Vector3 initialPosition;

	void Start () {
		initialPosition = transform.localPosition;
	}
	
	void Update () {
		float moveX = -Input.GetAxis("Mouse X") * sensitivity;
		float moveY = -Input.GetAxis("Mouse Y") * sensitivity;

		Vector3 finalPosition = new Vector3(moveX, moveY, 0);
		transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * speed);
	}
}
