using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    //public string interactEnum;
    public InteractType type;

    public void Push(Vector3 force, Vector3 point)
    {
        // Hit an object
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            // Add force to object
            rigid.AddForceAtPosition(force, point);
        }
    }

    //// Use this for initialization
    //protected virtual void Awake()
    //{
    //    interactEnum = this.gameObject.tag;
    //}

    //// Update is called once per frame
    //protected virtual void Update()
    //{
    //    interactType = (InteractType)System.Enum.Parse(typeof(InteractType), interactEnum);
    //}
}
