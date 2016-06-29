using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string name;
	public string iconName;
	public string description;

	public Item(string displayName, string iconAssetName, string descriptionText)
	{
		name = displayName;
		iconName = iconAssetName;
		description = descriptionText;
	}
}
