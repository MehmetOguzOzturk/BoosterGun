using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gun : MonoBehaviour
{
    bool canFire;
    bool isMoving;
    public int number = 0;
    public float bulletSpeed;
    Camera cam;
    Ray ray;
    public LayerMask ignore;
    public RaycastHit hit;
    public List<GameObject> enemylist = new List<GameObject>();
    public List<Transform> wayPoint;
    public GameObject bulletPrefab;
    public Transform firePoint;
    
    void Start()
    {
        cam = Camera.main;
        canFire = true;
        
    }

    
    void Update()
    {
        if (Input.GetMouseButton(0) && canFire)
        {
            StartCoroutine(Fire());
        }
        NextLevelMove();
        LookAtEnemy();
    }

    private void NextLevelMove()
    {
        if (enemylist.Count == 0)
        {
            // Sonraki levela geç.
            if (!isMoving)
            {
                if (number < wayPoint.Count)
                {
                    isMoving = true;
                    
                    transform.DOLookAt(wayPoint[number].position, 1f);
                    transform.DOMove(wayPoint[number].position, 4).SetDelay(1).OnComplete(()=>isMoving=false);
                    number++;
                  

                }
            }

        }
    }

    IEnumerator Fire()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        canFire = false;
        Vector3 direction;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity, ~ignore))
        {
            
            direction = hit.point - firePoint.position;
            direction = direction.normalized;
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);          
            bulletInstance.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
        else
        {
            
            
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody>().velocity = ray.direction * bulletSpeed;
        }
       
       
        yield return new WaitForSeconds(0.25f);
        canFire = true;
    }

    private void OnTriggerEnter(Collider other) // Düþmanlarý listeye ekle.
    {
        if (other.CompareTag("FindEnemies"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {             
                enemylist.Add(other.transform.GetChild(i).gameObject);
                other.transform.GetChild(i).tag = "member";
                other.transform.GetChild(i).GetComponent<Animator>().SetBool("walk", true);
                isMoving = false;
               
            }
        }
    }

    public void TakeDamage()
    {
        this.enabled = false;
    }

    void LookAtEnemy()
    {
        float distance;
        float minDistance = 10000;
        GameObject nearestEnemy=null;
        foreach (GameObject item in enemylist)
        {
            distance = Vector3.Distance(item.transform.position, transform.position);
            if (distance<minDistance)
            {
                nearestEnemy = item;
                minDistance = distance;
            }
        }
        if (enemylist.Count>0)
        {
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            Quaternion toRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
        }
        
    }
}
