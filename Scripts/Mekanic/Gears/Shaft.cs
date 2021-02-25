using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : Gear
{
    public override void setSourceGear8(Gear gear8)
    {
        this.transform.localRotation = gear8.transform.localRotation;
        this.currentRotation = gear8.currentRotation;

        this.angularVelocity = gear8.angularVelocity;
        this.source = gear8;
    }

    public override void setSourceGear16(Gear gear16)
    {
        this.transform.localRotation = gear16.transform.localRotation;
        this.currentRotation = gear16.currentRotation;

        this.angularVelocity = gear16.angularVelocity;
        this.source = gear16;
    }
}
