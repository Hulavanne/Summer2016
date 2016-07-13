using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class UniqueIdRegistry
{
    public static Dictionary<string, int> mapping = new Dictionary<string, int>();

    public static void Deregister(string id)
    {
        mapping.Remove(id);
    }

    public static void Register(string id, int value)
    {
        if (!mapping.ContainsKey(id))
        {
            mapping.Add(id, value);
        }
    }

    public static int GetInstanceId(string id)
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
