using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    EnemyBehavior parentScript;
    public GameObject enemyPlayer;
    public bool collisionCheck;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = GetComponentInParent<EnemyBehavior>();
    }

    // Update is called once per frame
   

    public IEnumerator CollisionCheckNumerator()
    {
        collisionCheck = true;
        yield return new WaitForEndOfFrame();
        collisionCheck = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet")) 
        {

            if (collisionCheck)
                return;

            List<Ragdoll> ragdollScript = new List<Ragdoll>(enemyPlayer.GetComponentsInChildren<Ragdoll>());
            foreach (var item in ragdollScript)
            {
                StartCoroutine(item.CollisionCheckNumerator());
            }

            parentScript.isShot = true;
            enemyPlayer.GetComponent<Animator>().enabled = false;
            parentScript.speed = 0;
            parentScript.enemyHealt -= 1;
            parentScript.GetEnemyPos();

          
            Debug.Log(transform.name);

        }
    }
}
