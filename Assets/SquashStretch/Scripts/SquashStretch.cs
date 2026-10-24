using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquashStretch : MonoBehaviour
{
    public Transform ball;
    public FloatSpring ballSpring = new() { frequencyHz = 2f, dampingRatio = 0.4f };
    bool _isPressed;
    // Vector3 _restBallPosition;
    // Vector3 _restBallScale;
    private Vector3 downScale = Vector3.one;
    private Vector3 upScale = Vector3.one;
    private float _velocity;
    private Vector3 newPosition;



    void Awake()
    {
        // _restBallPosition = transform.localPosition;
        // _restBallScale = transform.localScale;
        newPosition = transform.localPosition;
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
        transform.position = Vector3.MoveTowards(transform.position, newPosition, _velocity * Time.deltaTime);
        
        transform.localScale = Vector3.LerpUnclamped(upScale, downScale, ballSpring.Value);
    }
    

    void UpdateMouseState()
    {
        if (!Mouse.current.leftButton.isPressed)
            _isPressed = false;
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            _isPressed = Physics.Raycast(ray, out RaycastHit hitInfo);
            newPosition = hitInfo.point;

            // if (Physics.Raycast(ray, out RaycastHit hit))
            // {
            //     _isPressed = true;
            //     newPosition = hit.point;
            // }
        }
    }
    
}
