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

		RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.down), 10.0f, layerMask);

		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			IAbsorbable area = hit.transform.gameObject.GetComponent<AbsorptionArea>() as IAbsorbable;

			if (area != null)
			{
				return area;
			}
		}
		return null;
	}
}
