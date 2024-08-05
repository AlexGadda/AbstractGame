using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Animator settingAnim;
    [SerializeField] GameObject transition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSetting()
    {
        settingAnim.SetTrigger("Open");
    }

    public void CloseSetting()
    {
        settingAnim.SetTrigger("Close");
    }

    public void StartGame()
    {
        transition.SetActive(true);
        StartCoroutine(LoadSceneAfter(1, 1f));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAfter(int index, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(index);
    }    
}
