using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform HeadTransform;
    public GameObject BodyPart;

    private List<Transform> bodyPartTransforms;
    private List<Vector3> positions;
    private List<Quaternion> rotations;

    private int lastIndex;
    private float distance;
    public float bodyHorizontalSize;

    private void Start()
    {
        bodyPartTransforms = new List<Transform>();
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
        bodyHorizontalSize = BodyPart.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;

        positions.Add(HeadTransform.position);
        rotations.Add(HeadTransform.rotation);
    }

    public void AddPart()
    {
        lastIndex = positions.Count - 1;
        var newPart = Instantiate(BodyPart, positions[lastIndex], rotations[lastIndex], transform.parent);
        bodyPartTransforms.Add(newPart.transform);

        positions.Add(newPart.transform.position);
        rotations.Add(newPart.transform.rotation);
    }

    private void InsertTransform()
    {
        var direction = HeadTransform.position - positions[0];

        distance = direction.magnitude;

        if (distance > bodyHorizontalSize)
        {
            positions.Insert(0, positions[0] + direction.normalized * bodyHorizontalSize);
            positions.RemoveAt(positions.Count - 1);

            rotations.Insert(0, HeadTransform.rotation);
            rotations.RemoveAt(rotations.Count - 1);

            distance -= bodyHorizontalSize;
        }

        for (int i = 0; i < bodyPartTransforms.Count; i++)
        {
            bodyPartTransforms[i].position = Vector3.Slerp(positions[i + 1], positions[i], distance/bodyHorizontalSize);
            bodyPartTransforms[i].rotation = Quaternion.Slerp(rotations[i + 1], rotations[i], distance/bodyHorizontalSize);
        }
    }


    private void Update()
    {
        InsertTransform();
    }

}
