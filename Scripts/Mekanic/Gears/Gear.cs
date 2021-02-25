using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gear : MonoBehaviour
{
    public bool infinitSource = false;
    public int angularVelocity = 0;
    public Gear source; // infinitSource (in)dirctly rotating this gear 
    public List<Gear> links = new List<Gear>();
    private WorldMechanics worldMechanics;
    public float currentRotation;
    void Start()
    {
        this.worldMechanics = WorldMechanics.Instance;
        this.currentRotation = 0.0f;
    }
    void Update()
    {
        if (angularVelocity != 0)
        {
            float rotateAngle = angularVelocity * Time.deltaTime;
            transform.Rotate(Vector3.up * rotateAngle);
            this.currentRotation += rotateAngle;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (worldMechanics == null) return;
        // Debug.Log("collition " + name + " and " + other.name);
        Gear otherGear = other.gameObject.GetComponent<Gear>();
        if (otherGear == null) otherGear = other.gameObject.transform.parent.gameObject.GetComponent<Gear>();
        if (otherGear != null) handleGearCollision(otherGear);
    }

    void OnDestroy()
    {
        foreach (Gear link in links) link.links.Remove(this);

        worldMechanics.removeGear(this);
        worldMechanics.recompute();
    }

    private void handleGearCollision(Gear otherGear)
    {
        Debug.Log("Handle " + name + ", " + otherGear.name);
        this.links.Add(otherGear);
        if (otherGear.links.Contains(this))
        {
            worldMechanics.recompute();
        }
    }

    public bool isConflict(Gear otherSource)
    {
        // Require same speed
        if (otherSource.angularVelocity != source.angularVelocity)
        {
            Debug.Log(name + " conflict speed: " + source.angularVelocity + " != " + otherSource.angularVelocity);
            this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            this.angularVelocity = 0;
            return true;
        }
        // require rotation offset
        float anglePerCog = 360f / 8f;
        float soruceRelativeRotation = source.currentRotation;
        Debug.Log(name);
        float otherSourceRelativeRotation = otherSource.source.currentRotation;
        float offset = Mathf.Abs(soruceRelativeRotation + otherSourceRelativeRotation) % anglePerCog;
        if (offset != anglePerCog / 2)
        {
            Debug.Log(name + " conflict angle: " + Mathf.Round(offset) + " != " + (360f / 8f) / 2f);
            return true;
        }
        return false;
    }

    public void setSourceGear(Gear other)
    {
        switch (other.tag)
        {
            case "gear8":
                setSourceGear8(other);
                return;
            case "gear16":
                setSourceGear16(other);
                return;
            case "shaft":
                setSourceShaft(other);
                return;
            default:
                return;
        }
    }

    public abstract void setSourceGear8(Gear other);
    public abstract void setSourceGear16(Gear other);
    public void setSourceShaft(Gear shaft)
    {
        setRotation(shaft.currentRotation);
        int direction = getRotiationDirection(shaft, false);
        this.angularVelocity = direction * shaft.angularVelocity;
        this.source = shaft;
    }


    public int getRotiationDirection(Gear othergear, bool invert)
    {
        // check if gears have same normal
        bool sameDirection = !invert;
        if ((transform.up + othergear.transform.up) == Vector3.zero)
        {
            sameDirection = !sameDirection;
        }

        return sameDirection ? 1 : -1;
    }
    protected void setRotation(float rotation)
    {
        float rotateAngle = rotation - currentRotation;
        this.transform.Rotate(Vector3.up * rotateAngle);
        this.currentRotation += rotateAngle;
    }
}