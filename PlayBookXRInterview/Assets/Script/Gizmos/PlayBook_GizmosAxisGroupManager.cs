using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBook_GizmosAxisGroupManager : MonoBehaviour
{
    [SerializeField] private GizmosType.AxisDir axisDirection;
    [SerializeField] private GameObject transformHandle;
    [SerializeField] private GameObject scaleHandle;
    [SerializeField] private GameObject rotationHandle;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }
    
    private void Initialization()
    {
        switch (axisDirection)
        {
            case GizmosType.AxisDir.X:
                ChangeGizmosColor(Color.red);
                break;
            case GizmosType.AxisDir.Y:
                ChangeGizmosColor(Color.green);
                break;
            case GizmosType.AxisDir.Z:
                ChangeGizmosColor(Color.blue);
                break;
        }
    }

    private void ChangeGizmosColor(Color color)
    {
        transformHandle.GetComponent<Renderer>().material.color = color;
        rotationHandle.GetComponent<Renderer>().material.color = color;
        scaleHandle.GetComponent<Renderer>().material.color = color;
    }

    public GizmosType.AxisDir GetAxisDirection()
    {
        return axisDirection;
    }

}
