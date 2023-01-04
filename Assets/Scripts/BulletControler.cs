using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControler : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    private float bulletSpeed = 100f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }
    private void OnEnable()
    {
        Destroy(gameObject,timeToDestroy);
    }
   

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed*Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.01)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);

    }
}
