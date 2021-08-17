using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
	[SerializeField] private Material capMaterial;
	public GameObject Slice(bool reverse, Vector3 knifePosition , Vector3 knifeDirection)
    {
		var taken = new GameObject();
		transform.position = knifePosition;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.up, out hit))
		{
			GameObject victim = hit.collider.gameObject;

			GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, knifeDirection, capMaterial, reverse);
			taken = pieces[0];
			taken.name = "TakenBrick";
			pieces[1].name = "LostBrick";
			pieces[1].transform.position = new Vector3(pieces[1].transform.position.x, victim.transform.position.y, pieces[1].transform.position.z);
			if (!taken.GetComponent<Rigidbody>())
			{
				taken.AddComponent<Rigidbody>();
				taken.GetComponent<Rigidbody>().isKinematic = true;
			}
			if (!pieces[1].GetComponent<Rigidbody>())
			{
				pieces[1].AddComponent<Rigidbody>();
				pieces[1].GetComponent<Rigidbody>().isKinematic = false;
			}
			//adding mesh collider with actual brick size to perform collision properly
			var takenCollider = taken.GetComponent<MeshCollider>();
			takenCollider.sharedMesh = taken.GetComponent<MeshFilter>().mesh;
			takenCollider.convex = true;

			//adding mesh collider to perform bricks collision properly 
			pieces[1].AddComponent<MeshCollider>();
			var piece1Collider = pieces[1].GetComponent<MeshCollider>();
			piece1Collider.convex = true;

			//Destroy(pieces[1], 1);
		}
		return taken;

    }
	void OnDrawGizmosSelected()
	{

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);

	}
}
