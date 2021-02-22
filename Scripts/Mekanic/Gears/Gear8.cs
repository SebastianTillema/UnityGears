using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear8 : Gear
{
    public override void setSourceGear(Gear otherGear)
    {   
        switch (otherGear.tag)
        {
            case "gear8":
                setSourceGear8(otherGear);
                return;
            case "gear16":
                setSourceGear16(otherGear);
                return;
            case "shaft":
                setSourceShaft(otherGear);
                return;
            default:
                return;
        }
    }

    private void setSourceShaft(Gear shaft)
    {   
        Debug.Log("hey");
        this.snapShaft(shaft);
        // do we agree on up or are they opisit
        Vector3 a1 = this.transform.up;
        Vector3 a2 = shaft.transform.up;
        if ((this.transform.up + shaft.transform.up) == Vector3.zero)
            this.angularVelocity = -shaft.angularVelocity; // do not agree
        else
            this.angularVelocity = shaft.angularVelocity;

        this.source = shaft;
    }
    private void snapShaft(Gear shaft)
    {
        float rotateAngle = shaft.currentRotation - currentRotation;
        this.transform.Rotate(Vector3.up * rotateAngle);
        this.currentRotation += rotateAngle;
    }

    private void setSourceGear8(Gear otherGear8)
    {
        this.snapGear8(otherGear8);
        int direction = getRotiationDirection(otherGear8, false);
        this.angularVelocity =  -otherGear8.angularVelocity;
        this.source = otherGear8;
    }
    private void snapGear8(Gear otherGear8)
    {
        float anglePerCog = 360 / 8;
        float angleDifference = angleDifference = 0; //Should always be n*90 for only non diogonal binding 
        float goalAngle = -(otherGear8.currentRotation + 180) - 1 * angleDifference - anglePerCog / 2;

        float rotateAngle = goalAngle - currentRotation;
        this.transform.Rotate(Vector3.up * rotateAngle);
        this.currentRotation += rotateAngle;
    }
    private void setSourceGear16(Gear otherGear16)
    {
        this.snapGear16(otherGear16);
        int direction = getRotiationDirection(otherGear16, true);
        this.angularVelocity = direction * (2 * otherGear16.angularVelocity);
        this.source = otherGear16;
    }
    private void snapGear16(Gear otherGear16)
    {
        float anglePerCog = 360 / 16;
        Vector3 center = transform.position;
        Vector3 other_center = otherGear16.transform.position;
        float angleDifference = Mathf.Atan2(center.y - other_center.y, other_center.x - center.x) * 180.0f / Mathf.PI;
        float goalAngle = -(2 * otherGear16.currentRotation + 180 - 3 * angleDifference) - anglePerCog;

        float rotateAngle = goalAngle - currentRotation;
        this.transform.Rotate(Vector3.up * rotateAngle);
        this.currentRotation += rotateAngle;
    }

    /** 
        Returns 1 if this and other should rotate the same direction
        and -1 otherwise
    */
    private int getRotiationDirection(Gear othergear, bool invert)
    {
        // check if gears have same normal
        bool sameDirection = !invert;
        if ((transform.up + othergear.transform.up) == Vector3.zero)
        {
            sameDirection = !sameDirection;
        }

        Vector3 origin = Vector3.one - transform.up;
        Vector3 otherOrigin = Vector3.one - othergear.transform.up;
        if (origin != otherOrigin)
        {
            sameDirection = !sameDirection;
        }

        return sameDirection ? 1 : -1;
    }
}
