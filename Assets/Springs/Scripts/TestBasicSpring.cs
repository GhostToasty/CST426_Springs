using UnityEngine;

public class TestBasicSpring : MonoBehaviour
{
    [Header("Scene References")]
    public Transform equilibriumTransform;
    public Transform attachmentTransform;
    public LineRenderer springCoilLine;

    [Header("Spring Settings")]
    public float stiffness = 100f; // k value
    public float mass = 1f; // m value
    public float damping = 0.1f; // c value

    [Header("Debug")]
    public LineRenderer maxLine;
    public LineRenderer minLine;
    public float maxDisplacement;
    float _maxX, _minX;
    Vector3 _velocity;
  

    void Update()
    {
        // Step the spring in fixed 1/120 s chunks, however long the frame was.
        SimulateSpring(Time.deltaTime);
        UpdateVisuals();
    }

    void SimulateSpring(float deltaTime)
    {
        Vector3 currentPos = attachmentTransform.position;

        // TODO: Find the restoring force, which pulls toward equilibriumTransform by stiffness,
        // and the damping force, which opposes the velocity. Keep the velocity in a private
        // _velocity field. Acceleration is the total force divided by mass.
        Vector3 displacement = currentPos - equilibriumTransform.position;
        Vector3 restoringForce = displacement * stiffness * -1;
        Vector3 dampingForce = damping * _velocity * -1;
        
        Vector3 force = restoringForce + dampingForce;

        // TODO: Take a semi-implicit Euler step: update the velocity first, then move with the new
        // velocity.
        Vector3 acceleration = force / mass;
        _velocity = acceleration * deltaTime;
        attachmentTransform.position += _velocity * deltaTime;
    }

    void UpdateVisuals()
    {
        maxLine.positionCount = minLine.positionCount = 2;
        springCoilLine.SetPosition(0, equilibriumTransform.position);
        springCoilLine.SetPosition(1, attachmentTransform.position);

        Vector3 currentPos = attachmentTransform.position;
        Vector3 displacement = currentPos - equilibriumTransform.position;
        maxDisplacement = Mathf.Max(maxDisplacement, displacement.magnitude);

        if (currentPos.x > _maxX)
        {
            _maxX = currentPos.x;
            maxLine.SetPosition(0, currentPos + Vector3.up * 10f);
            maxLine.SetPosition(1, currentPos + Vector3.down * 10f);
        }

        if (currentPos.x < _minX)
        {
            _minX = currentPos.x;
            minLine.SetPosition(0, currentPos + Vector3.up * 10f);
            minLine.SetPosition(1, currentPos + Vector3.down * 10f);
        }
    }
}
