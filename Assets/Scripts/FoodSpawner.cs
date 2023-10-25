using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject Food;
    public MeshFilter Surface;

    public int startFoodCount = 100;

    private List<Vector3> vertices = new List<Vector3>();
    private float foodMaxSize;

    public void Start()
    {
        Surface.sharedMesh.GetVertices(vertices);
        Spawn(startFoodCount);

        var foodSize = Food.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        foodMaxSize = Mathf.Max(Mathf.Max(foodSize.x, foodSize.y), foodSize.z);
    }

    public void Spawn(int count)
    {
        if (count > vertices.Count)
            return;

        for (int i = 0; i < count; i++)
        {
            int j = Random.Range(0, vertices.Count - 1);

            if (CheckFreePosition(vertices[j]))
            {
                Instantiate(Food.gameObject, vertices[j], Quaternion.identity, transform);
            }
            else
            {
                count++;
            }
        }
    }

    private bool CheckFreePosition(Vector3 pos)
    {
        var colliders = Physics.OverlapSphere(pos, foodMaxSize);

        return colliders.Length == 0 ? true : false;
    }

}
