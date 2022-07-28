using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Class] PlayerController
 * 플레이어의 움직임을 관리합니다.
 */
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 3f;

	private Rigidbody rig;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 moveDir = new Vector3(h * moveSpeed * Time.deltaTime, 0f, v * moveSpeed * Time.deltaTime);
		rig.MovePosition(rig.position + moveDir);
	}
}
