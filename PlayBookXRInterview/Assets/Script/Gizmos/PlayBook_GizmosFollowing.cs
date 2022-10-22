using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBook_GizmosFollowing : MonoBehaviour
{
    public Transform _target { get; set; }
    

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position;
    }
}
