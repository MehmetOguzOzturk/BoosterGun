using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    Rigidbody rb;
    bool collisionCheck;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionCheck)
            return;

        collisionCheck = true;
        

        transform.parent = collision.gameObject.transform;
        GetComponent<SphereCollider>().enabled = false;
        if (!collision.gameObject.GetComponent<ConstantForce>() && collision.gameObject.CompareTag("enemyParts"))
        {
            collision.gameObject.AddComponent<ConstantForce>().relativeForce = new Vector3(0, 130, 0);
        }
       
        rb.isKinematic = true;
        
        if (collision.gameObject.CompareTag("box"))
        {
            
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward*10, ForceMode.Impulse);
        }
    }
    
}
