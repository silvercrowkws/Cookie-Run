using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Parent : MonoBehaviour
{
    public GameObject groundPrefabs;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Instantiate(groundPrefabs, transform.position, Quaternion.identity, transform);
    }
}
