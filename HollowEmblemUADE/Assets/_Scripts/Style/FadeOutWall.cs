using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWall : MonoBehaviour
{
    public GameObject hideParent;
    bool mustFade;
    public List<SpriteRenderer> sprites=new List<SpriteRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        var spritesArray = (hideParent.GetComponentsInChildren<SpriteRenderer>());
        foreach (SpriteRenderer sr in spritesArray)
        {
            sprites.Add(sr);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Fade();
        }
        if(mustFade)
        {
            StartCoroutine(Awaiting());
        }
    }
    public void Fade()
    {
        mustFade = true;
    }

    private IEnumerator Awaiting()
    {
        // Colocar el audio del ascensor
        
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            foreach (SpriteRenderer sr in sprites)
            {
                sr.color = new Color(255, 255, 255, i);
            }

            yield return null;
        }
        mustFade = false;
    }
}
