using UnityEngine;

public class MovingController : MonoBehaviour
{
    public LayerMask Mask;
    public float Velocity = 1;
    public float Height = 0.05f;
    public float CamHeight;
    public float LerpSpeed;
    public FloatingJoystick joystick;

    public Vector3 direction;

    [SerializeField]private Transform MeshTransform;
    [SerializeField]private Transform camTransform;

    private Quaternion rotation;
    private RaycastHit hit;
    private Vector3 movementDirection;
    private Vector3 destinationPoint;

    private void Start()
    {
        var y = transform.up * CamHeight;
        camTransform.position = transform.position + y;
        camTransform.LookAt(transform, camTransform.up);
    }

    private void Update()
    {
        ChangeDirection();
        CalculateDestination();
        CheckHeight();
        Move();
    }

    private void CalculateDestination()
    {
        movementDirection = transform.forward * Velocity;
        destinationPoint = transform.position + movementDirection * Time.deltaTime;
    }

    private void CheckHeight()
    {
        var direction = MeshTransform.position - destinationPoint;
      
        Physics.Raycast(destinationPoint, direction, out hit, 5, Mask);

        var cross = Vector3.Cross(hit.normal, transform.forward);//vector perpendicular to plane normal-forward for build new vector forward
        var newForward = Vector3.Cross(cross, hit.normal);

        var rotation = Quaternion.LookRotation(newForward,hit.normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LerpSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        var vertical = joystick.Vertical;
        var horizontal = joystick.Horizontal;

        if(vertical !=0 || horizontal !=0)
        {
            var forward = horizontal * camTransform.right + vertical * camTransform.up;
            rotation = Quaternion.LookRotation(forward, -camTransform.forward);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5 * LerpSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        var targetPoint = hit.point + hit.normal * Height;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, Velocity * Time.deltaTime);

        camTransform.position = Vector3.Lerp(camTransform.position, hit.point + hit.normal * CamHeight, LerpSpeed * Time.deltaTime);
        camTransform.LookAt(transform,camTransform.up);
    }


}
