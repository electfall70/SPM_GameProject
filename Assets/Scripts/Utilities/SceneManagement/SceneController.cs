using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController scenController;

    private void OnEnable() => EventHandler<DeathEvent>.RegisterListener(Restart);
    private void OnDisable() => EventHandler<DeathEvent>.UnregisterListener(Restart);

    private void Restart(DeathEvent eve)
    {
        if(eve.Info.unit.tag == "DeathScreen")
        {
            SceneManager.UnloadSceneAsync("Whitebox - 16April");
            StartCoroutine(LoadScene("Whitebox - 16April"));
            


            /*
             * unload all scenes
             * foreach(scene s i currentCheckpoint.relevantScenes)
             *  LoadAsync(s.name)
             *  
             */
        }
        
    }


    public void UnloadScene(/*n�nting f�r att identifiera vilken scen, string eller buildindex?*/)
    {

    }

    IEnumerator LoadScene(string sceneName)
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
        EventHandler<ReloadEvent>.FireEvent(new ReloadEvent(gameObject));
    }
}
