#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections.Generic;

// Placeholder for UniqueIdDrawer script
public class UniqueIdentifierAttribute : PropertyAttribute { }

[ExecuteInEditMode]
public class UniqueId : MonoBehaviour
{
    [UniqueIdentifier]
    public string uniqueId;

    void Start ()
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
            uniqueId = Guid.NewGuid().ToString();
            UniqueIdRegistry.Register(uniqueId, GetInstanceID());
        }

        #endif
    }
}