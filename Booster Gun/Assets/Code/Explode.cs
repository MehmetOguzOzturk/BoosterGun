using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float radius;
    public ParticleSystem explode;
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            explode.gameObject.transform.parent = null;
            explode.Play();
            ExplodeDamage();
            gameObject.SetActive(false);
        }
    }
    void ExplodeDamage() 
    {
       Collider[] colliders= Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("member"))
            {
                colliders[i].GetComponent<EnemyBehavior>().enemyHealt = 0;
            }
        }
    }
}
