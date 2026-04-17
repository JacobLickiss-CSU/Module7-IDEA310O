using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class HealthMonitor : MonoBehaviour
{
    public List<Texture> visualState = new List<Texture>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance != null)
        {
            SetVisual(((float)PlayerManager.Instance.CurrentHealth) / ((float)PlayerManager.Instance.MaxHealth));
        }
    }

    private void SetVisual(float healthPercent)
    {
        int index = (int) MathF.Round(((float)(visualState.Count-1) * healthPercent), 0, MidpointRounding.AwayFromZero);
        index = Math.Clamp(index, 0, visualState.Count - 1);

        Texture visual = visualState[index];
        GetComponent<RawImage>().texture = visual;
    }
}
