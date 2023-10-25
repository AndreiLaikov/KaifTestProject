using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodSpawner foodSpawnerInstance;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BodyController>().AddPart();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foodSpawnerInstance.Spawn(1);
    }
}
