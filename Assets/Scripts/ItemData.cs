using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemData
{
    public string id;
    public string dataStatus;
    public bool collected;

    [HideInInspector]
    public int charges;

    public ItemData(string itemID, int itemCharges)
	{
        id = itemID;
        dataStatus = "Original Data";
        collected = false;

        charges = itemCharges;
	}
}
