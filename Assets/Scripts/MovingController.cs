using UnityEngine;

public class MovingController : MonoBehaviour
{
    public LayerMask mask;
    public float velocity = 1;
    public Vector3 movementDirection;
    public Vector3 destinationPoint;
    public float minHeight = 0.2f;
    public float RotationSpeed = 0.1f;

    RaycastHit hit;

    public bool isMoving;
    public Vector3 RotationDirection;

    private void Update()
    {

        DestinationCalculate();
        CheckHeight();
        Move();
        Rotate();
        Debug.DrawRay(hit.point, hit.normal * 3, Color.magenta);
    }

    private void DestinationCalculate()
    {
        //movementDirection = transform.forward; //move with const speed

        movementDirection = Input.GetAxis("Vertical") * transform.forward; //move  with  buttons
        destinationPoint = transform.position + movementDirection * velocity * Time.deltaTime;
    }

    private void CheckHeight()
    {
        Physics.Raycast(destinationPoint,-transform.up, out hit, 2, mask); //get hit near destinationPoint but with "height"
    }

    private void Move()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, hit.point + transform.up * minHeight, velocity * Time.deltaTime);
            transform.rotation *= Quaternion.FromToRotation(transform.up, hit.normal);
        }
    }

    private void Rotate()
    {
    }
}
