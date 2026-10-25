using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquashStretch : MonoBehaviour
{
    public Transform ball;
    public FloatSpring ballSpring = new() { frequencyHz = 2f, dampingRatio = 0.4f };
    bool _isPressed;
    public Vector3 initialScale = Vector3.one;
    private Vector3 _newPosition;


    void Awake()
    {
        initialScale = transform.localScale;
        _newPosition = transform.position;
    }

    void Update()
    {
        UpdateMouseState();

        if (_isPressed)
            ballSpring.equilibrium = 1;
        else
            ballSpring.equilibrium = 0;
        
        ballSpring.Advance(Time.deltaTime);

        UpdateBallVisual();
    } 
    
    void UpdateBallVisual()
    {
        //goes from current position to new position 
        transform.position = Vector3.LerpUnclamped(transform.position, _newPosition, ballSpring.Value);

        //scaled so that object volume stays consistent during squash and stretch
        float stretchFactor = Mathf.Clamp(1f, 2f, Mathf.Abs(ballSpring.Value));
        float squashFactor = 1f / stretchFactor;
        Vector3 newScale = new Vector3(initialScale.x * squashFactor, initialScale.y * stretchFactor, initialScale.z * squashFactor);

        //rotates so stretched part is facing towards the new position 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, _newPosition), ballSpring.Value);
        
        //applies the squash and stretch scale 
        transform.localScale = Vector3.LerpUnclamped(initialScale, newScale, ballSpring.Value);
    }

    void UpdateMouseState()
    {
        if (!Mouse.current.leftButton.isPressed)
            _isPressed = false;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {            
            //uses raycast to detect where mouse has been clicked 
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());            
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _isPressed = true;
                _newPosition = hit.point;
            }
        }
    }
}