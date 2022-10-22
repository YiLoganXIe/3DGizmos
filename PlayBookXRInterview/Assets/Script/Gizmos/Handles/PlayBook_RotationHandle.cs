using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayBook_RotationHandle : MonoBehaviour, IPlayBookHandle
{
    [SerializeField] private float sensitivity = 10;
    private GizmosType.AxisDir _axisDir;
    private Material _material;
    private Color _originalColor;
    private bool _selected = false;
    private Transform _target;
    private Camera _gizmosCamera;
    private Vector3 _positivePosition;
    private Vector3 _negativePosition;
    private Vector3 _mouseStartPoint;
    private Vector3 _originalDirection;
    private float prevDelta;


    private void Start()
    {
        var manager = transform.parent.gameObject.GetComponent<PlayBook_GizmosAxisGroupManager>();
        _material = gameObject.GetComponent<Renderer>().material;
        _target = transform.parent.parent.gameObject.GetComponent<PlayBook_GizmosFollowing>()._target;
        _gizmosCamera = GameObject.Find("GizmosCamera").GetComponent<Camera>();
        _axisDir = manager.GetAxisDirection();
    }

    public void OnHover()
    {
        _originalColor = _material.color;
        _material.color = Color.yellow;
    }

    public void OnHoverExit()
    {
        _material.color = _originalColor;
    }

    // Initialize The scaling
    public void OnMousePressDown(Vector3 mouseHitPoint)
    {
        _mouseStartPoint = mouseHitPoint;
        _selected = true;
        InitializeTarget();
    }
    
    public void OnMouseRelease()
    {
        _selected = false;
        transform.parent.parent.rotation = _target.rotation;
    }
    

    private void OnMouseDrag()
    {
        if (_selected)
        {
            ProcessRotation();
        }
    }

    private void ProcessRotation()
    {
        Vector3 hitPoint = GetHitPointOnAxis();

        // Comparing the distance of two target on the opposite sides to determine the value is negative or not
        // Distance(PositiveTarget, InteractionOrigin) == Distance(NegativeTarget, InteractionOrigin)
        // If current cursor is on the positive side of origin, then Distance(CursorPos, PositiveTarget) always less
        // than Distance(CursorPos, NegativeTarget)
        float positiveDistance = Vector3.Distance(hitPoint, _positivePosition);
        float negativeDistance = Vector3.Distance(hitPoint, _negativePosition);
        float deltaRotation = Vector3.Distance(hitPoint, _mouseStartPoint) ;
        if (positiveDistance > negativeDistance)
        {
            deltaRotation = -deltaRotation;
        }

        deltaRotation *= sensitivity;
        
        switch (_axisDir)
        {
            case GizmosType.AxisDir.X:
                _target.Rotate(prevDelta-deltaRotation, 0, 0, Space.Self);
                break;
            case GizmosType.AxisDir.Y:
                _target.Rotate(0, prevDelta-deltaRotation, 0, Space.Self);
                break;
            case GizmosType.AxisDir.Z:
                _target.Rotate(0, 0, prevDelta-deltaRotation, Space.Self);
                break;
            
        }

        prevDelta = deltaRotation;


    }

    // Calculate the Cursor Value on the Axis of This Handle
    // Projecting Player Mouse Position on to the Target Axis.
    private Vector3 GetHitPointOnAxis()
    {
        Ray cameraRay = _gizmosCamera.ScreenPointToRay(Input.mousePosition);
        Ray handleRay = new Ray(_mouseStartPoint, _positivePosition);
        float closestT = PlayBook_GizmosMath.ClosestPointOnRay(handleRay, cameraRay);
        Vector3 hitPoint = handleRay.GetPoint(closestT);
        return hitPoint;
    }

    // Create Position Target as a Indicator of Positive Negative Value
    private void InitializeTarget()
    {
        // Scale Handle's forward is Red Arrow - transform.right
        _positivePosition = _mouseStartPoint + transform.right * 10;
        _negativePosition = _mouseStartPoint - transform.right * 10;
        
    }
}
