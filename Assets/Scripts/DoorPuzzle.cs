using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoorPuzzle : MonoBehaviour
{
    public static DoorPuzzle current;

    public List<Sprite> deactiveSprites = new List<Sprite>(new Sprite[5]);
    public List<Sprite> activeSprites = new List<Sprite>(new Sprite[5]);
    public List<int> correctSequence = new List<int>(new int[5]);
    public List<int> sequence = new List<int>();

    List<Button> buttons = new List<Button>();
    MenuController inGameUi;

	void Awake()
    {
        current = this;

        buttons = GetComponentsInChildren<Button>().ToList();
        inGameUi = GameObject.Find("InGameUI").GetComponent<MenuController>();

        Activate(false);
	}

	void Update()
    {
        if (sequence.Count == 5)
        {
            bool correct = true;

            for (int i = 0; i < correctSequence.Count; ++i)
            {
                if (correctSequence[i] != sequence[i])
                {
                    correct = false;
                }
            }

            if (correct)
            {
                EventManager.current.OpenDoorPuzzle();
                Activate(false);
            }
            else
            {
                Reset();
            }
        }
	}

    public void ButtonPressed(Button button)
    {
        int index = int.Parse(button.name[button.name.Count() - 1].ToString()) - 1;
        sequence.Add(index);

        button.transform.GetChild(0).GetComponent<Image>().sprite = activeSprites[index];
        button.interactable = false;
    }

    public void Activate(bool active)
    {
        // Activate / deactivate all children
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }

        // Pause game
        if (active)
        {
            Reset();
            inGameUi.PauseGame();
            inGameUi.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Resume game
        else
        {
            inGameUi.ResumeGame();
            inGameUi.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        sequence.Clear();

        for (int i = 0; i < deactiveSprites.Count; ++i)
        {
            buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = deactiveSprites[i];
            buttons[i].interactable = true;
        }
    }

    public void PlayButtonSoundEffect()
    {

    }
}
