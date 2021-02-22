using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Construct : MonoBehaviour, IPointerDownHandler
{   
    private float precision = 0.01f;
    public GameObject mechanics;
    public GameObject mechanicsObjectsParent;
    public GameObject itemBar;
    private List<GameObject> mechanicObjects = new List<GameObject>();
    private WorldMechanics worldMechanics;

    private int id = 0;
    void Start()
    {
        // Util.newScale(objToPlace1, new Vector3(10.0f, 10.0f, 10.0f));
        worldMechanics = (WorldMechanics)mechanics.GetComponent(typeof(WorldMechanics));
        foreach (Transform child in mechanicsObjectsParent.transform)
        {
            mechanicObjects.Add(child.gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitobj = hit.transform.gameObject;
            hit.normal = hitobj.transform.up; // object normal = up, assumtion 

            int activeItem = itemBar.GetComponent<ItemBar>().activeItem;
            if (activeItem >= mechanicObjects.Count)
            {
                Destroy(hitobj);
                return;
            }

            Vector3 pos = getGridPosition(hit);
            Quaternion rotation = getGridRotation(hit);

            GameObject gear = Instantiate(mechanicObjects[activeItem], pos, rotation);
            gear.transform.SetParent(mechanics.transform, true);
            gear.GetComponent<Gear>().worldMechanics = worldMechanics;
            gear.name = "" + id;
            id++;

            worldMechanics.addGear((Gear)gear.GetComponent(typeof(Gear)));
        }
    }

    private Vector3 getGridPosition(RaycastHit hit)
    {
        Vector3 hitPos = mechanics.transform.InverseTransformPoint(hit.point);
        Vector3 pos = hitPos;
        pos -= 5 * hit.normal;
        pos = new Vector3(Mathf.Round(pos.x / 10) * 10, Mathf.Round(pos.y / 10) * 10, Mathf.Round(pos.z / 10) * 10);
        pos += 5 * hit.normal;
        pos = updateInsetInGrid(hit.normal, hitPos, pos);
        return pos;
    }

    private Vector3 updateInsetInGrid(Vector3 normal, Vector3 hitPos, Vector3 pos) {
        float inset = 2.5f;
        if (normal.x > precision) pos.x = hitPos.x + inset;
        else if (-normal.x > precision) pos.x = hitPos.x - inset;
        else if (normal.y > precision) pos.y = hitPos.y + inset;
        else if (-normal.y > precision) pos.y = hitPos.y - inset;
        else if (normal.z > precision) pos.z = hitPos.z + inset;
        else if (-normal.z > precision) pos.z = hitPos.z - inset;
        return pos;
    }

    private Quaternion getGridRotation(RaycastHit hit)
    {
        Quaternion rotation = Quaternion.identity;

        // if roof flip 180
        if (hit.normal.y == -1) rotation *= Quaternion.AngleAxis(180f, Vector3.forward);

        // rotate object normal to hit normal 
        Vector3 rotationVector = new Vector3(hit.normal.z, 0, -hit.normal.x);
        rotation *= Quaternion.AngleAxis(90f, rotationVector);
        return rotation;
    }
}
