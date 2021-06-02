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
        if(eve.Info.hitComponent.CompareTag("DeathScreen"))
        {
            //H�mta alla scener som �r relevanta f�r checkpoint
            List<int> relevantScenes = Checkpoint.currentCheckPoint.ScenesOnCheckpoint;

            //ifall relevanta scener inte inneh�ller n�gon av de aktiva scenera s� laddas de av.
            for(int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).buildIndex != baseSceneIndex)
                {
                    UnloadScene(SceneManager.GetSceneAt(i).buildIndex);
                }
            }


            //om det finns n�gon relevant scen som inte �r laddad s� laddas den in
            foreach (int s in relevantScenes)
            {
                if (SceneManager.GetSceneByBuildIndex(s).isLoaded == false)
                    LoadScene(s);
            }


            //This should not be done like this but i cant get the IEnumerators to work like i want so i just wait fro an arbitrary time until i reload the player
            Invoke(nameof(CompleteReload), 2.5f);
            
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

    public void LoadScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index).isLoaded == false)
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }

    public void UnloadScene(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index).isLoaded == true)
            SceneManager.UnloadSceneAsync(index);
    }

    private void CompleteReload() { EventHandler<ReloadEvent>.FireEvent(new ReloadEvent()); }

    IEnumerator UnloadSceneIE(int index)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(index);
        //Scriptet f�r g� vidare n�r scenen laddats f�rdigt
        

        //koden �r fast h�r s� l�nge scenen inte laddat klart (skriva ut n�n progress??)
        while (asyncOperation.progress <= 0.89f)
        {
            yield return null;
        }

    }

    IEnumerator LoadSceneIE(int index)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        //Scriptet f�r g� vidare n�r scenen laddats f�rdigt
        asyncOperation.allowSceneActivation = false;
        
        //koden �r fast h�r s� l�nge scenen inte laddat klart (skriva ut n�n progress??)
        while (asyncOperation.progress <= 0.89f)
        {
            yield return null;
        }
        Debug.Log(SceneManager.GetSceneByBuildIndex(index).name + " is DONE");
        asyncOperation.allowSceneActivation = true;

    }
    /*
    IEnumerator Reload()
    {
        AsyncOperation[] scenesToLoad = new AsyncOperation[Checkpoint.currentCheckPoint.ScenesOnCheckpoint.Count];
        AsyncOperation[] scenesToUnload = new AsyncOperation[SceneManager.sceneCount];

        

        for(int i = 0; i < scenesToUnload.Length; i++)
        {
            
        }


        for (int i = 0; i < RestOfScenes.Length; i++)
        {
            if (SceneManager.GetSceneByName(RestOfScenes[i].SceneName).isLoaded)
            {
                asyncUnloadRestOfScenes[i] = SceneManager.UnloadSceneAsync(RestOfScenes[i].SceneName);
                while (!asyncUnloadRestOfScenes[i].isDone)
                {
                    Debug.Log("UNLOADING: " + RestOfScenes[i].SceneName);
                    yield return null;
                }
                Debug.Log("LOADING");
                asyncLoadRestOfScenes[i] = SceneManager.LoadSceneAsync(RestOfScenes[i].SceneName, LoadSceneMode.Additive);
            }
            else
            {

                Debug.Log("LOADING: " + RestOfScenes[i].SceneName);
                asyncLoadRestOfScenes[i] = SceneManager.LoadSceneAsync(RestOfScenes[i].SceneName, LoadSceneMode.Additive);

            }
            asyncLoadRestOfScenes[i].allowSceneActivation = false;
        }
    }
    */
}
