using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour
{
    public float speed ;
    bool isRange;
    public bool isShot;
    public int enemyHealt;

    Animator anim;
    public Transform target;
    public Gun gunScirpt;

    [SerializeField] float distance;
    [SerializeField] List<Transform> childTransforms;
    [SerializeField] List<Vector3> childBonePosition;
    [SerializeField] List<Vector3> childBoneEuler;
    private void Start()
    {
        
        enemyHealt = 2;
        anim = GetComponent<Animator>();
        isShot = false;
        isRange = false;

        childTransforms = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        childTransforms.Remove(transform);

        foreach (var item in childTransforms)
        {
            if (item.GetComponent<Rigidbody>())
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().useGravity = false;
            }
            
        }

    }

    private void Update()
    {
        if (!isShot && gameObject.tag == "member" && !isRange)
        {
            speed = 1;        
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z), Vector3.up);
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        }

        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 3.5f && !isRange)
        {
            speed = 0;
            anim.SetBool("attack", true);
            isRange = true;
        }

        if (enemyHealt <= 0 ) // Düþmanlarý listeden çýkart.
        {     
            gunScirpt.enemylist.Remove(gameObject);
            StopAllCoroutines();
            isShot = true;
            anim.enabled = false;
            speed = 0;
            GetComponent<CapsuleCollider>().enabled = false;
            foreach (var item in childTransforms)
            {
                if (item.GetComponent<Rigidbody>())
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                    item.GetComponent<Rigidbody>().useGravity = true;
                }

            }
           this.enabled = false;
        }

    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Gun"))
        {
            gunScirpt.TakeDamage();
        }

        if (isShot && enemyHealt > 0)
        {
            StartCoroutine(StandUp());
        }
    }

    public void GetEnemyPos()
    {
        foreach (var item in childTransforms)
        {
            if (item.GetComponent<Rigidbody>())
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.GetComponent<Rigidbody>().useGravity = true;
            }
            childBonePosition.Add(item.localPosition);
            childBoneEuler.Add(item.localEulerAngles);
        }
    }

    public IEnumerator StandUp()
    {
        yield return new WaitForSeconds(4);
        
        isShot = false;
        for (int i = 0; i < childTransforms.Count; i++)
        {
            childTransforms[i].DOLocalMove(childBonePosition[i], 1);
            childTransforms[i].DOLocalRotate(childBoneEuler[i], 1);
        }
        yield return new WaitForSeconds(1);
        anim.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        foreach (var item in childTransforms)
        {
            if (item.GetComponent<Rigidbody>())
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

    }

  
    
       
    


    



}
