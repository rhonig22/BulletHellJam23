using UnityEditor;
using UnityEngine;
using System.Collections;

//this class should exist somewhere in your project
public class Shield : MonoBehaviour
{
    [SerializeField] GameObject shield;
    private float _shieldPercent = 1f;
    public float ShieldPercent {
        get
        {
            return _shieldPercent;
        }
        set
        {
            _shieldPercent = value;
            shield.transform.localScale = new Vector3(value, value, 1);
        }
    }
}