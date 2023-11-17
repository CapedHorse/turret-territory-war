using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int BulletTeamId;
    public MeshRenderer BulletMesh;
    public Rigidbody BulletRb;

    public void InitiateBullet(int id, Color color)
    {
        BulletTeamId = id;
        BulletMesh.material.color = color;
    }

    public void Launch(Vector3 direction)
    {
        BulletRb.AddForce(direction * 50f);
    }
    
    public void Despawn()
    {
        BulletTeamId = 0;
        BulletMesh.material.color = Color.white;
        LeanPool.Despawn(gameObject);
    }
}
