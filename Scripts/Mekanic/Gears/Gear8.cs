using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear8 : Gear
{
    public override void setSourceGear8(Gear otherGear8)
    {
        float anglePerCog = 360 / 8;
        float goalAngle = -(otherGear8.currentRotation + 180) + anglePerCog / 2;
        setRotation(goalAngle);

        int direction = getRotiationDirection(otherGear8, true);
        this.angularVelocity = direction * otherGear8.angularVelocity;
        this.source = otherGear8;
    }

    public override void setSourceGear16(Gear otherGear16)
    {
        float anglePerCog = 360 / 16;
        Vector3 center = transform.position;
        Vector3 other_center = otherGear16.transform.position;
        float angleDifference = Mathf.Atan2(center.y - other_center.y, other_center.x - center.x) * 180.0f / Mathf.PI;
        float goalAngle = -(2 * otherGear16.currentRotation + 180 - 3 * angleDifference) - anglePerCog;
        setRotation(goalAngle);

        int direction = getRotiationDirection(otherGear16, true);
        this.angularVelocity = direction * (2 * otherGear16.angularVelocity);
        this.source = otherGear16;
    }
}
