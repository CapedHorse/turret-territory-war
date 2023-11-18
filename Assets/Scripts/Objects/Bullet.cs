using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Lean.Pool;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{

    public int BulletTeamId;
    public MeshRenderer BulletMesh;
    public Rigidbody BulletRb;

    private float speed = 0;
    private int bounceHealth = 3;

    public void InitiateBullet(int id, Color color)
    {
        BulletTeamId = id;
        BulletMesh.material.color = color;
        bounceHealth = 3;
    }

    public void Launch(Vector3 direction, float launchSpeed)
    {
        BulletRb.velocity = direction * launchSpeed;
        speed = launchSpeed;
    }
    
    public void Despawn()
    {
        BulletTeamId = 0;
        BulletMesh.material.color = Color.white;
        BulletRb.velocity = Vector3.zero;
        LeanPool.Despawn(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            //Reduce health everytime bounce wall, remove this if you want the ball to keep bouncing infinitely
            bounceHealth--;
            if (bounceHealth <= 0)
            {
                Despawn();
            }
            
            //Bounce wall to the oposite direction
            if (other.transform.name == "NorthWall") {
                if (BulletRb.velocity.x < 0 )  {
                    BulletRb.velocity = new Vector3 (-1 * speed,0,-1 * speed);
                } else {
                    BulletRb.velocity = new Vector3 (speed,0,-1 * speed);
                }
       
            }
   
            if (other.transform.name == "SouthWall") {
                if (BulletRb.velocity.x < 0 )  {
                    BulletRb.velocity = new Vector3 (-1 * speed,0,speed);
                } else {
                    BulletRb.velocity = new Vector3 (speed,0,speed);
                }
       
            }
   
            if (other.transform.name == "WestWall") {
                if (BulletRb.velocity.z < 0 )  {
                    BulletRb.velocity = new Vector3 (speed,0,-1 * speed);
                } else {
                    BulletRb.velocity = new Vector3 (speed,0,speed);
                }
       
            }
   
            if (other.transform.name == "EastWall") {
                if (BulletRb.velocity.z < 0 )  {
                    BulletRb.velocity = new Vector3 (-1 * speed,0,-1 * speed);
                } else {
                    BulletRb.velocity = new Vector3 (-1 * speed,0,speed);
                }
       
            }
        }
    }
}
