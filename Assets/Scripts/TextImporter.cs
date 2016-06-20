using UnityEngine;
using System.Collections;

public class TextImporter : MonoBehaviour {

    public TextAsset textFile;
    public string[] textLines;

	void Awake () {
	
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        else
        {

        }

	}
	
	
}
