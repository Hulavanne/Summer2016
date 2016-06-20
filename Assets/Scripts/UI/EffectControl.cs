using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffectControl : MonoBehaviour {

    ParticleSystem warpForm;
    ParticleSystem speedUp;
    ParticleSystem hyper;
    ParticleSystem slowDown;
    
    GameObject tunnel;
    Text countdown;
    VolControl sound;

    float time;
    int timeSecsLeft;
    bool counting;
    bool hasJumped;    
    bool inHyper;

	void Start ()
    {
        warpForm = GameObject.Find("warpFormEffect").GetComponent<ParticleSystem>();
        speedUp = GameObject.Find("jumpEffect").GetComponent<ParticleSystem>();
        slowDown = GameObject.Find("slowdownEffect").GetComponent<ParticleSystem>();
        hyper = GameObject.Find("hyperTubeEffect").GetComponent<ParticleSystem>();
        hasJumped = false;
        sound = GameObject.FindGameObjectWithTag("statistics").GetComponent<VolControl>();
        countdown = GameObject.Find("countDown").GetComponent<Text>();
        countdown.text = "";
        counting = true;
        time = 0;
        sound.CountDown();
	}
	
	void Update () 
    {
        time += Time.deltaTime;
        timeSecsLeft = 5 - (int)time;
        countdown.text = timeSecsLeft.ToString();

        if (!hasJumped && time >= 4.0f)
        {            
            Warp();
        }
        if (counting && time >= 5.0f)
        {
            countdown.gameObject.SetActive(false);
            counting = false;
        }
        // Back to normal space criteria here. NB! slowing effect has to be called earlier.

	}

    void Warp()
    {
        warpForm.Play();
        speedUp.Play();      
        hasJumped = true;
        inHyper = true;
        Invoke("SetHyperEffect", 1.2f);
    }
    public void SlowDown()
    {        
        
        sound.Disengage();
        inHyper = false;
        Invoke("SetHyperEffect", 1.1f);      
    }
    void SetHyperEffect()
    {
        if (!inHyper)
        {
            slowDown.Play();
        }
        hyper.enableEmission = inHyper;       
    }
}
