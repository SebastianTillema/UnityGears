using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void newScale(GameObject theGameObject, Vector3 newSize)
    {

        float size = theGameObject.GetComponent<Renderer>().bounds.size.y;

        Vector3 rescale = theGameObject.transform.localScale;

        rescale.x = newSize.x * rescale.x / size;
        rescale.y = newSize.y * rescale.y / size;
        rescale.z = newSize.z * rescale.z / size;

        theGameObject.transform.localScale = rescale;

    }
}
