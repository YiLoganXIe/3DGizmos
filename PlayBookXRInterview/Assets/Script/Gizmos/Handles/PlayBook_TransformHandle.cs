using System;
using UnityEngine;

public class PlayBook_TransformHandle : MonoBehaviour, IPlayBookHandle
{
    private GizmosType.AxisDir _axisDir;
    private Material _material;
    private Color _originalColor;
    private bool _selected = false;
    private Transform _target;
    private Vector3 _targetOriginalPosition;
    private Camera _gizmosCamera;
    private Vector3 _positivePosition;
    private Vector3 _negativePosition;
    private Vector3 _mouseStartPoint;


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
        _targetOriginalPosition = _target.position;
        _mouseStartPoint = mouseHitPoint;
        _selected = true;
        InitializeTarget();
    }
    
    public void OnMouseRelease()
    {
        _selected = false;
    }

    private void Update()
    {
        if (_selected)
        {
            ProcessTransition();
        }
    }
    
    private void ProcessTransition()
    {
        Vector3 hitPoint = GetHitPointOnAxis();

        // Comparing the distance of two target on the opposite sides to determine the value is negative or not
        // Distance(PositiveTarget, InteractionOrigin) == Distance(NegativeTarget, InteractionOrigin)
        // If current cursor is on the positive side of origin, then Distance(CursorPos, PositiveTarget) always less
        // than Distance(CursorPos, NegativeTarget)
        float positiveDistance = Vector3.Distance(hitPoint, _positivePosition);
        float negativeDistance = Vector3.Distance(hitPoint, _negativePosition);
        float deltaDistanceTransition = Vector3.Distance(hitPoint, _mouseStartPoint) ;
        if (positiveDistance > negativeDistance)
        {
            deltaDistanceTransition = -deltaDistanceTransition;
        }
        switch (_axisDir)
        {
            case GizmosType.AxisDir.X:
                _target.position = _targetOriginalPosition + _target.right * deltaDistanceTransition;
                break;
            case GizmosType.AxisDir.Y:
                _target.position = _targetOriginalPosition + _target.up * deltaDistanceTransition;
                break;
            case GizmosType.AxisDir.Z:
                _target.position = _targetOriginalPosition + _target.forward * deltaDistanceTransition;
                break;
            
        }
        
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
        // Scale Handle's forward is Blue Arrow - transform.forward
        _positivePosition = _mouseStartPoint + transform.forward * 10;
        _negativePosition = _mouseStartPoint - transform.forward * 10;
    }
}
