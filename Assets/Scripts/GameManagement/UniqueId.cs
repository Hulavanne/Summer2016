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

    void Start ()
    {
        #if UNITY_EDITOR

        if (String.IsNullOrEmpty(this.uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
        }
        UniqueIdRegistry.Register(this.uniqueId, this.GetInstanceID());

        #endif
    }

    void OnDestroy()
    {
        #if UNITY_EDITOR

        UniqueIdRegistry.Deregister(this.uniqueId);

        #endif
    }

    void Update()
    {
        #if UNITY_EDITOR

        if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
            UniqueIdRegistry.Register(this.uniqueId, this.GetInstanceID());
        }

        #endif
    }
}