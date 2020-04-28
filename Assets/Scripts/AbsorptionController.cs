using UnityEngine;

public class AbsorptionController : MonoBehaviour
{
	public bool isTesting = false;

	void Update()
	{
		if (isTesting && Input.GetKeyDown(KeyCode.Space))
		{
			TryAbsorption();
		}
	}

	private bool TryProcessUnderneath(bool isAbsorption)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f, layerMask))
		{
			IAbsorbable area = hit.transform.gameObject.GetComponent<AbsorptionArea>() as IAbsorbable;
			if (area != null)
			{
				if (isAbsorption)
				{
					return area.OnAbsorption();
				}
				else
				{
					return area.OnRestore();
				}
			}
		}
		return false;
	}

	public bool TryAbsorption()
	{
		return TryProcessUnderneath(true);
		
	}

	public bool TryRestore()
	{
		return TryProcessUnderneath(false);
	}
}
