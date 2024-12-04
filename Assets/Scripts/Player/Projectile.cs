using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    
    private bool collided;

    private void OnEnable()
    {
        transform.GetChild(0).DOPunchPosition(new Vector3(Random.Range(-1,1), Random.Range(-1,1), Random.Range(-1,1)), 0.5f, 4);
        Destroy(gameObject,2f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(collided)
            return;
        
        collided = true;
        
        if (other.gameObject.CompareTag("Ground"))
        {
            var impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            
            Destroy(impact,0.4f);
            Destroy(gameObject);
        }
    }
}