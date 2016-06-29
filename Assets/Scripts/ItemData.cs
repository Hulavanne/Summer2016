using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemData : MonoBehaviour
{
	public string displayName;
	public Sprite icon;
	public string description;

	public Item item;

	void Awake()
	{
		item = new Item(displayName, icon.name, description);
	}
}
