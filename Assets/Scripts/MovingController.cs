using UnityEngine;

public class MovingController : MonoBehaviour
{
    public LayerMask Mask;
    public float Velocity = 1;
    public float Height = 0.05f;
    public float CamHeight;
    public float LerpSpeed;

    [SerializeField]private Transform MeshTransform;
    [SerializeField]private Transform camTransform;

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

        Debug.DrawRay(destinationPoint, direction, Color.green);
        Debug.DrawRay(hit.point, hit.normal * 5, Color.magenta);

        var cross = Vector3.Cross(hit.normal, transform.forward);//vector perpendicular to plane normal-forward for build new vector forward
        var newForward = Vector3.Cross(cross, hit.normal);

        var rotation = Quaternion.LookRotation(newForward,hit.normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, LerpSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if(Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            var forward = horizontal * camTransform.right + vertical * camTransform.up;
            var rotation = Quaternion.LookRotation(forward, -camTransform.forward);

            //transform.rotation = rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5*LerpSpeed * Time.deltaTime);
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
