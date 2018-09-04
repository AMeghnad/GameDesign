using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    public string interactEnum;
    public InteractType interactType;

    // Use this for initialization
    protected virtual void Awake()
    {
        interactEnum = this.gameObject.tag;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        interactType = (InteractType)System.Enum.Parse(typeof(InteractType), interactEnum);
    }
}
