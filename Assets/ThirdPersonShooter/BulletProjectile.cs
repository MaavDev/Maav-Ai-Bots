using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody bulletRigidbody;

    public float speed = 50f;
    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {

        bulletRigidbody.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<BulletTarget>() != null) {
            // Hit target
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        } else {
            // Hit something else
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}