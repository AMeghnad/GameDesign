using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactEnum;

    // Use this for initialization
    protected virtual void Awake()
    {
        interactEnum = this.gameObject.tag;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
