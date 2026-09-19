using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Themes / Music")]
    [field: SerializeField] public EventReference themes { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference pegHit { get; private set; }


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMODEvents in the scene.");
        }
        instance = this;
    }


}