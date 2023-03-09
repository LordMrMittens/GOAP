using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    CameraMovement cam;

    [SerializeField] GameObject escapePanel;
    private void Start() {
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if ((cam != null && !cam.isCloseUp) && escapePanel != null)
        {
            if (escapePanel.activeInHierarchy == true)
            {
                escapePanel.SetActive(false);
                WorldStatusManager.WSMInstance.ChangeWorldSpeed(1);
            }
            else
            {
                escapePanel.SetActive(true);
                WorldStatusManager.WSMInstance.ChangeWorldSpeed(0);
            }
        }
    }

    public void LoadScene(int sceneToLoad){
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
