using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public bool fixedCamera = false;

	LevelManager levelManager;

	void Awake()
	{
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		//levelManager.levels.Add(gameObject);
	}

	void Update()
	{
		
	}
}
