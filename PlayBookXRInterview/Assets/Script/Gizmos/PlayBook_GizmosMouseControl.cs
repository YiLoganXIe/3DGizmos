using UnityEngine;

public class PlayBook_GizmosMouseControl : MonoBehaviour
{
    [SerializeField] private float rayCastDistance;
    private Camera _camera;
    private RaycastHit hit;
    private IPlayBookHandle _currentHandle;
    private bool _mouseHold = false;
    private bool _hover = false;
    private Vector3 _mouseHitPoint;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _hover)
        {
            _mouseHold = true;
            _currentHandle?.OnMousePressDown(_mouseHitPoint);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _currentHandle?.OnMouseRelease();
            _mouseHold = false;
        }
    }


    // Fixed Update Handle Physics - Physics.RayCast
    private void FixedUpdate()
    {
        // Bit shift the index of the layer (3) - Gizmos to get a bit mask
        int layerMask = 1 << 3;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        // If Mouse Hover
        if (Physics.Raycast(ray, out hit, rayCastDistance, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            _mouseHitPoint = hit.point;
            if (!_mouseHold)
            {
                HoverUIColorChange(hitObject);
            }

            _hover = true;
        }
        else
        {
            if (!_mouseHold)
            {
                _currentHandle?.OnHoverExit();
            }

            _hover = false;
        }
    }

    private void HoverUIColorChange(GameObject hitObject)
    {
        _currentHandle?.OnHoverExit();
        _currentHandle = hitObject.GetComponent<IPlayBookHandle>();
        _currentHandle.OnHover();
    }


}
