using UnityEngine;

public class MovingController : MonoBehaviour
{
    public LayerMask mask;
    public float velocity = 1;
    public Vector3 movementDirection;
    private Vector3 destinationPoint;
    public float Height = 0.2f;
    public float camHeight;
    public Transform MeshTransform;

    public Transform camTransform;

    RaycastHit hit;

    private void Update()
    {
        ChangeDirection();
        CalculateDestination();
        CheckHeight();
        Move();
    }

    private void CalculateDestination()
    {
        movementDirection = transform.forward * velocity * Time.deltaTime;
        destinationPoint = transform.position + movementDirection;
    }

    private void CheckHeight()
    {
        var direction = MeshTransform.position - destinationPoint;
        Physics.Raycast(destinationPoint, direction, out hit, 5, mask);

        var cross = Vector3.Cross(hit.normal, transform.forward);
        var newFrw = Vector3.Cross(cross, hit.normal);

        var rot = Quaternion.LookRotation(newFrw,hit.normal);
        transform.rotation = rot;
    }

    private void ChangeDirection()
    {
        var ver = Input.GetAxis("Vertical");
        var hor = Input.GetAxis("Horizontal");

        if(Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            var vec = hor * camTransform.right + ver * camTransform.up;
            var rot = Quaternion.LookRotation(vec, -camTransform.forward);

            transform.rotation = rot;
        }
    }


    private void Move()
    {
        var targetPoint = hit.point + hit.normal * Height;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, velocity * Time.deltaTime);

        camTransform.position = hit.point + hit.normal * camHeight;
        camTransform.LookAt(transform,camTransform.up);
    }


}
