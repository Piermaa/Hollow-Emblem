using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AsyncSceneCharger : MonoBehaviour
{
    public Image loadingBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingGame());  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadingGame()
    {

        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Juan Map");
        //Don't let the Scene activate until you allow it to
       
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
          
            Debug.Log( "Loading progress: " + (asyncOperation.progress * 100) + "%");
            //LAGUEARLAESCENA();
            //Output the current progress
            loadingBar.fillAmount= (asyncOperation.progress);

            if (asyncOperation.progress >= 0.9f)
            {
                StartCoroutine(AwakeGame(asyncOperation));
                break;
            }
            yield return null;
        }
     

    }
    void LAGUEARLAESCENA()
    {
        var images=FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer i in images)
        {
            var lagarto = Instantiate(i.gameObject,i.gameObject.transform);
            print("ESPRITES:"+images.Length);
        }
         
    }
    IEnumerator AwakeGame(AsyncOperation loading)
    {
        //loadingBar.fillAmount = 0.93f;
        //yield return new WaitForSeconds(1);
        loadingBar.fillAmount = 1;
        loading.allowSceneActivation = true;
        yield return null;
    }
}
