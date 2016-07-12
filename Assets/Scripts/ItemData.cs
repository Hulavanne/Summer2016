using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemData
{
    public string id;
    public bool collected;

    public ItemData(string itemID, bool isCollected)
	{
        id = itemID;
        collected = isCollected;
	}
}
