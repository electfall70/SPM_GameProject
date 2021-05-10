using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController scenController;

    private void OnEnable()
    {
        EventHandler<LoadSceneEvent>.RegisterListener(LoadScene);
        EventHandler<UnloadSceneEvent>.RegisterListener(UnloadScene);
        EventHandler<DeathEvent>.RegisterListener(Restart);
    }
    private void OnDisable() 
    {
        EventHandler<LoadSceneEvent>.RegisterListener(LoadScene);
        EventHandler<UnloadSceneEvent>.RegisterListener(UnloadScene);
        EventHandler<DeathEvent>.UnregisterListener(Restart);
    }


    private void Restart(DeathEvent eve)
    {
        if(eve.Info.unit.tag == "DeathScreen")
        {
            SceneManager.UnloadSceneAsync("Whitebox - 16April");
            StartCoroutine(ReloadScenes("Whitebox - 16April"));
            EventHandler<ReloadEvent>.FireEvent(new ReloadEvent(gameObject));


            /*
             * unload all scenes except active sc ene
             * foreach(scene s i currentCheckpoint.relevantScenes)
             *  LoadAsync(s.name)
             *  
             */
        }
        
    }

    public void LoadScene(LoadSceneEvent eve)
    {
        if(!SceneManager.GetSceneByBuildIndex(eve.buildIndex).isLoaded)
            SceneManager.LoadSceneAsync(eve.buildIndex, LoadSceneMode.Additive);
    }

    public void UnloadScene(UnloadSceneEvent eve)
    {
        if(SceneManager.GetSceneByBuildIndex(eve.buildIndex).isLoaded)
            SceneManager.UnloadSceneAsync(eve.buildIndex);
    }

    IEnumerator ReloadScenes(string sceneName)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //Scriptet f�r g� vidare n�r scenen laddats f�rdigt
        asyncOperation.allowSceneActivation = true;
        
        //koden �r fast h�r s� l�nge scenen inte laddat klart (skriva ut n�n progress??)
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        
    }
}
