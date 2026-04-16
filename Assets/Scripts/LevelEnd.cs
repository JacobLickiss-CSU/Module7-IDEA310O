using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public string Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // TODO victory screen?
            DataManager.Instance.SaveState(Target);
            DataManager.Instance.LoadState(true);
        }
    }
}
