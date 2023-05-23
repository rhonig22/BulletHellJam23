using UnityEditor;
using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering.Universal;

//this class should exist somewhere in your project
public class Shield : MonoBehaviour
{
    [SerializeField] GameObject shield;
    private float _shieldPercent = 1f;
    private readonly float _shieldPercentOffset = .6f;
    private readonly float _shieldPercentRange = .7f;
    private readonly float _shieldMinAlpha = .35f;
    private readonly float _shieldMaxIntensity = .6f;
    public float ShieldPercent {
        get
        {
            return _shieldPercent;
        }
        set
        {
            _shieldPercent = value;
            UpdateShieldSize(_shieldPercent);
            UpdateShieldColor(_shieldPercent);
            UpdateShieldLight(_shieldPercent);
        }
    }

    private void UpdateShieldSize(float percent)
    {
        if (percent > 0)
            shield.transform.localScale = new Vector3(percent * _shieldPercentRange + _shieldPercentOffset, percent * _shieldPercentRange + _shieldPercentOffset, 1);
        else
            shield.transform.localScale = new Vector3(0, 0, 1);
    }

    private void UpdateShieldColor(float percent)
    {
        var sprite = shield.GetComponent<SpriteRenderer>();
        var color = sprite.material.color;
        color.a = percent * (1 - _shieldMinAlpha) + _shieldMinAlpha;
        sprite.material.color = color;
    }

    private void UpdateShieldLight(float percent)
    {
        var light = shield.GetComponentInChildren<Light2D>();
        light.intensity = percent* _shieldMaxIntensity;
    }
}