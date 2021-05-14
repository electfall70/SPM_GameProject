//Author: Rickard Lindgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController scenController;

    private int baseSceneIndex = 1;

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
            //H�mta alla scener som �r relevanta f�r checkpoint
            List<int> relevantScenes = Checkpoint.currentCheckPoint.ScenesOnCheckpoint;

            Scene[] activeScenes = SceneManager.GetAllScenes();

            //ifall relevanta scener inte inneh�ller n�gon av de aktiva scenera s� laddas de av.
            foreach(Scene sc in activeScenes)
            {
                if (relevantScenes.Contains(sc.buildIndex) == false && sc.buildIndex != baseSceneIndex)
                    SceneManager.UnloadSceneAsync(sc.name);
            }
            //om det finns n�gon relevant scen som inte �r laddad s� laddas den in
            foreach(int s in relevantScenes)
            {
                if (SceneManager.GetSceneByBuildIndex(s).isLoaded == false)
                    StartCoroutine(ReloadScene(s));
            }

            EventHandler<ReloadEvent>.FireEvent(new ReloadEvent());
            
        }
        
    }

    public void LoadScene(LoadSceneEvent eve)
    {
        if(SceneManager.GetSceneByBuildIndex(eve.buildIndex).isLoaded == false)
            SceneManager.LoadSceneAsync(eve.buildIndex, LoadSceneMode.Additive);
    }

    public void UnloadScene(UnloadSceneEvent eve)
    {
        if(SceneManager.GetSceneByBuildIndex(eve.buildIndex).isLoaded == true)
            SceneManager.UnloadSceneAsync(eve.buildIndex);
    }

    IEnumerator ReloadScene(int index)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        //Scriptet f�r g� vidare n�r scenen laddats f�rdigt
        asyncOperation.allowSceneActivation = true;
        
        //koden �r fast h�r s� l�nge scenen inte laddat klart (skriva ut n�n progress??)
        while (asyncOperation.isDone == false)
        {
            yield return null;
        }
        
    }
}
