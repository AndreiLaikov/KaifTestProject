using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BodyController>().AddPart();
        Destroy(gameObject);
    }
}
