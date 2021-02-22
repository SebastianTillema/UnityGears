using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear16 : Gear
{
        public override void setSourceGear(Gear otherGear)
    {
        switch (otherGear.tag)
        {
            case "gear8":
                setSourceGear8(otherGear);
                return;
            case "gear16":
                // setSourceGear16(otherGear); // not possible
                return;
            default:
                return;
        }
    }
    private void setSourceGear8(Gear otherGear8)
    {
        this.snapGear8(otherGear8);
        this.angularVelocity = -otherGear8.angularVelocity/2;
        this.source = otherGear8;
    }
    private void snapGear8(Gear otherGear8)
    {
        float anglePerCog = 360 / 16;
        Vector3 center = transform.position;
        Vector3 other_center = otherGear8.transform.position;
        float angleDifference = Mathf.Atan2(center.y - other_center.y, other_center.x - center.x) * 180.0f / Mathf.PI;
        
        float goalAngle = -otherGear8.currentRotation/2 + 3/2*angleDifference - 90 - anglePerCog/2;

        float rotateAngle = goalAngle - currentRotation; 
        this.transform.Rotate(Vector3.up * rotateAngle); 
        this.currentRotation += rotateAngle;
    }
}
