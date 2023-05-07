using UnityEditor;
using UnityEngine;
using System.Collections;

//this class should exist somewhere in your project
public class Shield : MonoBehaviour
{
    [SerializeField] GameObject shield;
    private float _shieldPercent = 1f;
    private readonly float _shieldPercentOffset = .6f;
    private readonly float _shieldPercentRange = .7f;
    private readonly float _shieldMinAlpha = .35f;
    public float ShieldPercent {
        get
        {
            return _shieldPercent;
        }
        set
        {
            _shieldPercent = value;
            if (value > 0)
                shield.transform.localScale = new Vector3(value* _shieldPercentRange + _shieldPercentOffset, value* _shieldPercentRange + _shieldPercentOffset, 1);
            else
                shield.transform.localScale = new Vector3(0, 0, 1);

            var sprite = shield.GetComponent<SpriteRenderer>();
            Debug.Log(sprite.material.color);
            var color = sprite.material.color;
            color.a = value * (1 - _shieldMinAlpha) + _shieldMinAlpha;
            sprite.material.color = color;
        }
    }
}