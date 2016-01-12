using UnityEngine;
using System.Collections;

public class ChildRotation : MonoBehaviour 
{
    Quaternion StartRotation;
    void Awake()
    {
        StartRotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = StartRotation;
    }
}
