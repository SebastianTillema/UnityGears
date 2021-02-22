using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMechanics : MonoBehaviour
{
    public List<Gear> gears;
    public Gear source;

    void Start()
    {
        gears = new List<Gear>();
    }

    public void recompute()
    {
        resetMechanics();
        recomputeRotations();
    }
    private void resetMechanics()
    {
        foreach (Gear gear in gears)
        {
            gear.source = null;
            gear.angularVelocity = 0;
            gear.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }

    private void recomputeRotations()
    {
        HashSet<Gear> visited = new HashSet<Gear>();
        HashSet<Gear> soruceSet = new HashSet<Gear>();
        Queue<Gear> queue = new Queue<Gear>();
        queue.Enqueue(source);

        while (queue.Count > 0)
        {
            Gear currentGear = queue.Dequeue();
            if (!visited.Add(currentGear)) continue;

            foreach (Gear link in currentGear.links)
            {
                if (link.infinitSource || visited.Contains(link)) continue;

                if (!soruceSet.Add(link))
                {
                    // check for conflict with other soruce
                    if (link.isConflict(currentGear)) Debug.Log("Conflict!");

                }
                else
                {
                    queue.Enqueue(link);
                    link.setSourceGear(currentGear);
                }
            }
        }
    }

    public void addGear(Gear gear)
    {
        this.gears.Add(gear);
    }

    public void removeGear(Gear gear)
    {
        this.gears.Remove(gear);
    }
}
