using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
public class RaycastShoot : MonoBehaviour
{
    public InputActionReference action;
    public bool projectileShoot = true;
    public GameObject prefab;
    public Transform spawnPosition;
    public float shootSpeed = 10;
    public float bulletLifetime = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //I want this function to run whenever I push the shoot button (defined in the input system)
    public void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            //first cast the ray out from the camera, in the way it is looking
            //this variable will store info of what we hit, if anything
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //if we hit something, tell me WHAT we hit.
            if (Physics.Raycast(ray, out hit) && !projectileShoot)
            {
                if(hit.collider != null)
                {
                    UnityEngine.Debug.Log(hit.collider.gameObject.name);
                    if(hit.collider.gameObject.GetComponent<EnemyHealth>() != null)
                    {
                        hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
                    }
                }
            }
            else
            {
                //this is where we want to spawn the projectile
                //our preferred destination will be the point where our raycast hits
                Vector3 dest = hit.point;
                if (hit.collider == null)
                {
                    dest = Camera.main.transform.position + Camera.main.transform.forward * shootSpeed;
                }
                GameObject bullet = Instantiate(prefab, spawnPosition.position, Quaternion.identity);
                Vector3 velocity = dest - spawnPosition.position;
                velocity.Normalize();
                bullet.GetComponent<Rigidbody>().linearVelocity = velocity * shootSpeed;
                Destroy(bullet, bulletLifetime);
            }
        }
    }
}
