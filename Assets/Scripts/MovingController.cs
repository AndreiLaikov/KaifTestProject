using UnityEngine;

public class MovingController : MonoBehaviour
{
    public float Velocity;
    public float CamHeight;
    public float CamLerpSpeed;
    public float JoysticSensitivity;
    public float CastPointOffset;

    
    [SerializeField]private FloatingJoystick joystick;
    [SerializeField]private LayerMask mask;

    private Transform camTransform;
    private Vector3 direction;
    private Quaternion rotation;
    private RaycastHit hit;
    private Vector3 destinationPoint;

    private void Start()
    {
        camTransform = Camera.main.transform;
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
        CamMove();
    }

    private void CalculateDestination()
    {
        destinationPoint = transform.position + transform.forward;
    }

    private void CheckHeight()
    {
        direction = -transform.up;

        Physics.Raycast(destinationPoint + transform.up, direction, out hit, 3, mask);

        var cross = Vector3.Cross(hit.normal, transform.forward);//vector perpendicular to plane normal-forward for build new vector forward
        var newForward = Vector3.Cross(cross, hit.normal);

        var rotation = Quaternion.LookRotation(newForward,hit.normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Velocity * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        var vertical = joystick.Vertical;
        var horizontal = joystick.Horizontal;

        if(vertical !=0 || horizontal !=0)
        {
            var forward = horizontal * camTransform.right + vertical * camTransform.up;
            rotation = Quaternion.LookRotation(forward, -camTransform.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, JoysticSensitivity);
        }
    }

    private void Move()
    {
        var targetPoint = hit.point;
        if (targetPoint == Vector3.zero)
        {
            targetPoint = transform.forward * Velocity * Time.deltaTime;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint, Velocity * Time.deltaTime);
    }

    private void CamMove()
    {
        camTransform.position = Vector3.Lerp(camTransform.position, transform.position + transform.up * CamHeight, CamLerpSpeed * Time.deltaTime);
        camTransform.LookAt(transform, camTransform.up);
    }


}
