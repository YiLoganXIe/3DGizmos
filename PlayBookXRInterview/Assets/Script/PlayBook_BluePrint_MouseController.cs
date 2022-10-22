using UnityEngine;
using UnityEngine.UI;

public class PlayBook_BluePrint_MouseController : MonoBehaviour
{
    [Tooltip("Distance of Object position from Camera on Spawn")]
    [SerializeField] private float distance;

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private GameObject gizmosParentPrefab;

    [HideInInspector] public Button spawnerButton;

    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MouseFollowing();
        SpawningOnClick();
    }

    void MouseFollowing()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 point = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane + distance));
        transform.position = point;
    }

    void SpawningOnClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var cube = Instantiate(objectPrefab, transform.position, transform.rotation);
            Instantiate(gizmosParentPrefab).GetComponent<PlayBook_GizmosFollowing>()._target = cube.transform;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        spawnerButton.interactable = true;
    }
}
