using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayBook_UI_BluePrintSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject cubeBluePrintPrefab;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var button = gameObject.GetComponent<Button>();
            // Make the Button not Interactable to Prevent Unexpected Behavior
            button.interactable = false;
            // Send the Button Reference to Make Button Interactable Once Creation Finished 
            Instantiate(cubeBluePrintPrefab).GetComponent<PlayBook_BluePrint_MouseController>().spawnerButton = button;
            
        }
    }
}
