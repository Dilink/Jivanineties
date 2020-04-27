using UnityEngine;

public class TestSphereController : MonoBehaviour
{
	public int ballspeed = 5;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		float Hmove = Input.GetAxis("Horizontal");

		float Vmove = Input.GetAxis("Vertical");

		Vector3 ballmove = new Vector3(Hmove, 0.0f, Vmove);

		rb.AddForce(ballmove * ballspeed);
	}
}
