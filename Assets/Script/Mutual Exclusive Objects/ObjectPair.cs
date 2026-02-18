using UnityEngine;

[System.Serializable]
public class ObjectPair : MonoBehaviour
{
    [SerializeField] private StationaryObject[] objects;

    void LateUpdate()
    {
        StationaryObject litAndVisibleObject = null;

        foreach (var obj in objects)
        {
            if (obj.IsLit && obj.IsVisible)
            {
                litAndVisibleObject = obj;
                break; // only consider the first lit object that is visible
            }
        }

        if (litAndVisibleObject == null)
        {
            foreach(var obj in objects)
            {
                if (!obj.IsVisible && obj.IsLit) continue;
                obj.SetVisibility(true);
            }
        } else {
            foreach (var obj in objects)
            {
                if (!obj.IsVisible && obj.IsLit) continue;
                if (obj == litAndVisibleObject)
                {
                    if (!obj.IsLit)
                        obj.SetVisibility(true); // lit object always visible
                }
                else
                {
                    // only hide objects that are not lit
                    if (!obj.IsLit)
                        obj.SetVisibility(false);
                }
            }
        }
    }
}
