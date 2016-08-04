using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoorPuzzle : MonoBehaviour
{
    public List<int> correctSequence = new List<int>(new int[5]);
    public List<int> sequence = new List<int>();

    List<Button> buttons = new List<Button>();

	void Awake()
    {
        buttons = GetComponentsInChildren<Button>().ToList();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
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
                Debug.Log("Correct sequence");
                Reset();
            }
            else
            {
                Debug.Log("Incorrect sequence");
                Reset();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            sequence.Clear();

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            Reset();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Reset();

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ButtonPressed(Button button)
    {
        int index = int.Parse(button.name[button.name.Count() - 1].ToString()) - 1;
        sequence.Add(index);
        button.interactable = false;
    }

    public void PlayButtonSoundEffect()
    {

    }

    void Reset()
    {
        sequence.Clear();

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
