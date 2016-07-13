using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class UniqueIdRegistry
{
    public static Dictionary<String, int> mapping = new Dictionary<String, int>();

    public static void Deregister(String id)
    {
        mapping.Remove(id);
    }

    public static void Register(String id, int value)
    {
        if (!mapping.ContainsKey(id))
        {
            mapping.Add(id, value);
        }
    }

    public static int GetInstanceId(String id)
    {
        if (mapping.ContainsKey(id))
        {
            return mapping[id];
        }
        else
        {
            return -1;
        }
    }
}
