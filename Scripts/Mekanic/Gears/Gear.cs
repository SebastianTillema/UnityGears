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
    public GameObject mechanics;
    public WorldMechanics worldMechanics;
    public float currentRotation;
    void Start()
    {
        if (worldMechanics == null)
        {
            this.worldMechanics = (WorldMechanics)mechanics.GetComponent(typeof(WorldMechanics));
        }
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
        Debug.Log("collition " + name + " and " + other.name);
        Gear otherGear = other.gameObject.GetComponent<Gear>();
        if (otherGear == null) other.gameObject.transform.parent.gameObject.GetComponent<Gear>();
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
        float otherSourceRelativeRotation = otherSource.source.currentRotation;
        float offset = Mathf.Abs(soruceRelativeRotation + otherSourceRelativeRotation) % anglePerCog;
        if (offset != anglePerCog / 2)
        {
            Debug.Log(name + " conflict angle: " + Mathf.Round(offset) + " != " + (360f / 8f) / 2f);
            return true;
        }
        return false;
    }

    public abstract void setSourceGear(Gear gear);
}