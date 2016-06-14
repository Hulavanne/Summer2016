using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingPointsSprite : MonoBehaviour {

    public Vector3 offset;
    public float lifeTime;

    private float dt;
    private float lifeTimer;
    private float currentAlpha;

    CanvasRenderer canvas;

    void Awake()
    {
        lifeTimer = lifeTime;
        currentAlpha = 1;
        canvas = this.GetComponent<CanvasRenderer>();
    }
	
	void Update () 
    {
        dt = Time.deltaTime;
        transform.position += offset * dt;
        //renderer.material.color = DeltaFade(renderer.material.color, dt);
        currentAlpha -= 1 / lifeTime * dt;
        canvas.SetAlpha(currentAlpha);

        lifeTimer -= dt;
        if (lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
	}
    Color DeltaFade(Color color, float _dt)
    {
        Color temp = color;
        temp.a -= 1 / lifeTime * _dt;
        color = temp;
        return color;
    }
}
