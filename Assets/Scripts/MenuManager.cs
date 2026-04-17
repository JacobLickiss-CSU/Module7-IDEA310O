using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject ContinueButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Release the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1.0f;

        if (!CanContinue() && ContinueButton != null)
        {
            ContinueButton.SetActive(false);
            // ContinueButton.GetComponent<Button>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame()
    {
        DataManager.Instance?.LoadState(true);
    }

    public void StartGame()
    {
        DataManager.Instance?.RestoreDefaults();
        ContinueGame();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public bool CanContinue()
    {
        return DataManager.Instance.SavedLevel != DataManager.Instance.DefaultLevel &&
            DataManager.Instance.SavedLevel != "MainMenu";
    }
}
