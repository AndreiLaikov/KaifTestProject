using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodSpawner foodSpawnerInstance;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BodyController>().AddPart();
        foodSpawnerInstance.Spawn(1);
        Destroy(gameObject);
    }
}
