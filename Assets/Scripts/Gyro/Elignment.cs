
using UnityEngine;

public class Elignment : MonoBehaviour
{



	private float Y, Z;
	void Update()
	{
		Y = (transform.rotation.eulerAngles.y) % 360;
		Z = (transform.rotation.eulerAngles.z) % 360;

		transform.rotation = Quaternion.Euler(90, Y, Z);
	}
}
