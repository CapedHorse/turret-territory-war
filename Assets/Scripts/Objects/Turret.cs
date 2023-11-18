using System;
using System.Collections;
using System.Collections.Generic;
using CapedHorse;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class Turret : MonoBehaviour, ITeamHolder
{

    public int turretTeamId;
    public Color turretTeamColor;
    // public GameObject turretMeshParent;
    public Bullet bulletPrefab;
    public Transform bulletRail;
    public float bulletSpeed = 1f;
    
    [SerializeField] bool allowMove;//, moveRight = true;

    [SerializeField] Quaternion maxRotRight, maxRotLeft;
    
    [SerializeField] float lerp, fixedPlayTime;
    
    
    void Start()
    {
        float angle = GameManager.instance.gameSettings.turretRotateDegree;
        // transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x,
        //     Random.Range(transform.localEulerAngles.y - angle, transform.localEulerAngles.y + angle), transform.localEulerAngles.z);

        maxRotLeft = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y- angle, transform.localEulerAngles.z );
        maxRotRight = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y+ angle, transform.localEulerAngles.z );

        StartCoroutine(DelayedStart());
    }
    
    public void InitiateTeam(int teamId, GameSettings.Team team)
    {
        turretTeamId = teamId;
        turretTeamColor = team.TeamColor;
        /*foreach (var mesh in turretMeshParent.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (var material in mesh.materials)
            {
                material.color = turretTeamColor;
            }
        }*/
    }


    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(Random.Range(0, 2));
        allowMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowMove)
        {
            //move to left at first
            fixedPlayTime += Time.fixedDeltaTime;
            lerp = .5f * (/*GameManager.instance.gameSettings.turretRotateSpeed */ 1
                           + Mathf.Sin(Mathf.PI * fixedPlayTime * 0.25f));
            transform.localRotation = Quaternion.Lerp(maxRotLeft, maxRotRight, lerp);
        }
    }

    public void ShootBullet()
    {
        Bullet spawnedBullet = LeanPool.Spawn(bulletPrefab, bulletRail.position, bulletRail.rotation);
        spawnedBullet.InitiateBullet(turretTeamId, turretTeamColor);
        spawnedBullet.Launch(bulletRail.forward, bulletSpeed);
        // Debug.Log("Shooting bullet by team "+turretTeamId, gameObject);
    }
}
