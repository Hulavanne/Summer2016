using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemData
{
    public int index;
    public bool used;

    public ItemData(int indexNumber, bool isUsed)
	{
        index = indexNumber;
        used = isUsed;
	}
}
