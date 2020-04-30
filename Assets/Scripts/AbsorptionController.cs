using UnityEngine;

public class AbsorptionController : MonoBehaviour
{
	public bool isTesting = false;

	void Update()
	{
		if (isTesting && Input.GetKeyDown(KeyCode.Space))
		{
			IAbsorbable area = GetArea();
			area?.OnAbsorption();
		}
	}

	public IAbsorbable GetArea()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f, layerMask))
		{
			IAbsorbable area = hit.transform.gameObject.GetComponent<AbsorptionArea>() as IAbsorbable;
			return area;
		}
		return null;
	}

}
