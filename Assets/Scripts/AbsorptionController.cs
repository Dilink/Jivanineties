using UnityEngine;

public class AbsorptionController : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			int layerMask = 1 << LayerMask.NameToLayer("Player");
			layerMask = ~layerMask;

			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f, layerMask))
			{
				var area = hit.transform.gameObject.GetComponent<AbsorptionArea>();
				if (area)
				{
					area.OnAbsorption();
				}
			}
		}
	}
}
