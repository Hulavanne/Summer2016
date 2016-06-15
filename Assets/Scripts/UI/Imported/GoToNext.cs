using UnityEngine;
using System.Collections;

public class GoToNext : MonoBehaviour
{
    public string SceneToGo;
    public int levelId;
    public LoadScreen loader;
    public VolControl sounds;

    void Awake()
    {
        //loader = GameObject.FindWithTag("statistics").GetComponent<LoadScreen>();
        //sounds = GameObject.FindWithTag("statistics").GetComponent<VolControl>();
    }
	public void GoToScene()
    {
        //loader.showLoader(SceneToGo, levelId);
        //Application.LoadLevel(SceneToGo);
    }

}
