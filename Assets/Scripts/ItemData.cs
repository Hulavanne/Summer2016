using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemData
{
    public string id;
    public string dataStatus;
    public bool collected;

    public ItemData(string itemID, bool isCollected)
	{
        id = itemID;
        dataStatus = "Original Data";
        collected = isCollected;
	}
}
