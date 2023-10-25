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

    private void Start()
    {
        bodyPartTransforms = new List<Transform>();
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        positions.Add(HeadTransform.position);
        rotations.Add(HeadTransform.rotation);

        AddPart();
    }

    [ContextMenu("Add")]
    private void AddPart()
    {
        lastIndex = positions.Count - 1;

        var newPart = Instantiate(BodyPart, positions[lastIndex], rotations[lastIndex]);
        bodyPartTransforms.Add(newPart.transform);
        positions.Add(newPart.transform.position);
        rotations.Add(newPart.transform.rotation);
    }

    private void InsertTransform()
    {
        distance = (HeadTransform.position - positions[0]).magnitude;

        if (distance > 1.0f)
        {
            positions.Insert(0, HeadTransform.position);
            positions.RemoveAt(positions.Count - 1);

            rotations.Insert(0, HeadTransform.rotation);
            rotations.RemoveAt(rotations.Count - 1);

            distance -= 1;

        }

        for (int i = 0; i < bodyPartTransforms.Count; i++)
        {
            bodyPartTransforms[i].position = Vector3.Lerp(positions[i + 1], positions[i], distance);
            bodyPartTransforms[i].rotation = Quaternion.Lerp(rotations[i + 1], rotations[i], distance);
        }
    }


    private void Update()
    {
        InsertTransform();
        if (Input.GetKeyDown(KeyCode.Space))
            AddPart();
    }

}