using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShatterDoor : MonoBehaviour
{
    public TextMeshPro Label = null;

    public List<GameObject> Protectors = new List<GameObject>();

    private bool Broken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Broken)
        {
            for (int i = 0; i < Protectors.Count; i++)
            {
                if (Protectors[i] == null)
                {
                    Protectors.RemoveAt(i);
                    break;
                }
            }
            UpdateLabel();

            if (Protectors.Count <= 0)
            {
                Shatter();
            }
        }
    }

    void Shatter()
    {
        Destroy(GetComponent<BoxCollider>());
        foreach (Transform sherd in transform)
        {
            sherd.gameObject.AddComponent(typeof(Rigidbody));
        }
        Broken = true;
    }

    void UpdateLabel()
    {
        if (Label != null)
        {
            Label.text = "x" + Protectors.Count;
        }
    }
}
