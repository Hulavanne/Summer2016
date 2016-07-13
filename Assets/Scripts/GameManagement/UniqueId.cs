using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

// Placeholder for UniqueIdDrawer script
public class UniqueIdentifierAttribute : PropertyAttribute { }

[ExecuteInEditMode]
[RequireComponent (typeof (Item))]
public class UniqueId : MonoBehaviour
{
    [UniqueIdentifier]
    public string uniqueId;

    //public List<string> strings = new List<string>();
    //public List<int> ints = new List<int>();

    /*void Start ()
    {
        #if UNITY_EDITOR

        if (String.IsNullOrEmpty(uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
        }
        UniqueIdRegistry.Register(uniqueId, GetInstanceID());

        #endif
    }

    void OnDestroy()
    {
        #if UNITY_EDITOR

        UniqueIdRegistry.Deregister(uniqueId);

        #endif
    }

    void Update()
    {
        #if UNITY_EDITOR

        if(EditorApplication.isPlaying )
        {
            return;
        }

        if (GetInstanceID() != UniqueIdRegistry.GetInstanceId(uniqueId))
        {
            Debug.Log("NULL or EMPTY");
            uniqueId = Guid.NewGuid().ToString();
            UniqueIdRegistry.Register(uniqueId, GetInstanceID());
        }

        //strings.Clear();
        //ints.Clear();

        //foreach (string str in UniqueIdRegistry.mapping.Keys)
        //{
        //    strings.Add(str);
        //}
        //foreach (int integer in UniqueIdRegistry.mapping.Values)
        //{
        //    ints.Add(integer);
        //}

        #endif
    }*/
}