using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRaycastSelect : PlayerAdditions
{
    Color oldColor;

    public Color selectionColor;

    protected override void OnRaycastEnter(GameObject target)
    {
        //oldColor = target.GetComponent<Renderer>().material.GetColor("_Color");
        //target.GetComponent<Renderer>().material.SetColor("_Color", selectionColor);
    }

    protected override void OnRaycastExit(GameObject target)
    {
        base.OnRaycastExit(target);
        //target.GetComponent<Renderer>().material.SetColor("_Color", oldColor);
    }
}
