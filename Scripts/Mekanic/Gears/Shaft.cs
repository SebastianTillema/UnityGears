using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : Gear
{
    public override void setSourceGear(Gear gear)
    {
        switch (gear.tag)
        {
            case "gear8":
                setSourceGear8(gear);
                return;
            case "gear16":
                return;
            default:
                return;
        }
    }

    private void setSourceGear8(Gear gear8)
    {
        this.snapGear8(gear8);
        this.angularVelocity = gear8.angularVelocity;
        this.source = gear8;
    }
    private void snapGear8(Gear gear8)
    {
        this.transform.localRotation = gear8.transform.localRotation;
        this.currentRotation = gear8.currentRotation;
    }
}
