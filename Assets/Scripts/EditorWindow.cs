using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor; //using System.Linq;

namespace CarrotOnFire.Assets.Editor
{
    public class CarrotFindReferencesHelper : MonoBehaviour
    {
        [MenuItem("CONTEXT/Component/Find References to Component")]
        private static void FindReferences(MenuCommand data) { Object context = data.context; if (context) { var comp = context as Component; if (comp) FindReferencesTo(comp); } }

        [MenuItem("GameObject/Find References to Selected", false, 0)]

        public static void FindReferencesToAsset()
        {
            var selected = Selection.activeObject;
            if (selected)
                FindReferencesTo(selected);
        }
        private static void FindReferencesTo(Object to)
        {
            var referencedBy = new List<Object>();
            var allObjects = Object.FindObjectsOfType<GameObject>();
            bool toIsGameObject = to is GameObject;
            Component[] toComponents = toIsGameObject ? ((GameObject)to).GetComponents<Component>() : null;
            string toName = toIsGameObject ? to.name : string.Format("{0}.{1}", to.name, to.GetType().Name);
            for (int j = 0; j < allObjects.Length; j++)
            {
                GameObject go = allObjects[j];
                if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                {
                    if (PrefabUtility.GetPrefabParent(go) == to)
                    {
                        Debug.Log(string.Format("referenced by {0}, {1}", go.name, go.GetType()), go);
                        referencedBy.Add(go);
                    }
                }
                var components = go.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    var component = components[i];
                    if (!component) continue;
                    var so = new SerializedObject(component);
                    var sp = so.GetIterator();
                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            if (sp.objectReferenceValue == to)
                            {
                                Debug.Log(string.Format("'{0}' referenced by '{1}' (Component: '{2}')", toName, component.name, component.GetType().Name), component);
                                referencedBy.Add(component.gameObject);
                            }
                            else if (toComponents != null)
                            {
                                bool found = false;
                                foreach (Component toComponent in toComponents)
                                {
                                    if (sp.objectReferenceValue == toComponent)
                                    {
                                        found = true;
                                        referencedBy.Add(component.gameObject);
                                    }
                                }
                                if (found)
                                    Debug.Log(string.Format("'{0}' referenced by '{1}' (Component: '{2}')", toName, component.name, component.GetType().Name), component);
                            }
                        }
                    }
                }
            }
            if (referencedBy.Count > 0)
                Selection.objects = referencedBy.ToArray();
            else Debug.Log(string.Format("'{0}': no references in scene", toName));
        }
    }
}