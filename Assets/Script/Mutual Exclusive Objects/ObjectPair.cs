using UnityEngine;

[System.Serializable]
public class ObjectPair : MonoBehaviour
{
    [SerializeField] private StationaryObjectGroup[] objectGroups;

    void LateUpdate()
    {
        StationaryObjectGroup litAndVisibleObject = null;

        foreach (var obj in objectGroups)
        {
            if (obj.IsLit && obj.IsVisible)
            {
                litAndVisibleObject = obj;
                break; // only consider the first lit object that is visible
            }
        }

        if (litAndVisibleObject == null)
        {
            foreach(var obj in objectGroups)
            {
                if (!obj.IsVisible && obj.IsLit) continue;
                obj.SetVisibility(true);
            }
        } else {
            foreach (var obj in objectGroups)
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

[System.Serializable]
public class StationaryObjectGroup
{
    public StationaryObject[] objects;

    public bool IsLit => CheckIsLit();
    public bool IsVisible => CheckIsVisible();

    private bool CheckIsLit()
    {      
        foreach (StationaryObject obj in objects){
            if (obj.IsLit) return true;
        }
        return false;
    }

    private bool CheckIsVisible()
    {      
        foreach (StationaryObject obj in objects){
            if (obj.IsVisible) return true;
        }
        return false;
    }


    public void SetLit(bool lit)
    {
        foreach (StationaryObject obj in objects){
            obj.SetLit(lit);
        }
    }

    public void SetVisibility(bool vis)
    {
        foreach (StationaryObject obj in objects){
            obj.SetVisibility(vis);
        }
        
    }
}