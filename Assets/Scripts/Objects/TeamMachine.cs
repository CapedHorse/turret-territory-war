using System;
using System.Collections;
using System.Collections.Generic;
using CapedHorse;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TeamMachine : MonoBehaviour, ITeamHolder
{
    public int BulletProducerTeamId;
    public Color BulletProducerColor;
    public Transform BulletStartPoint, BulletEndPoint;

    public Turret TeamTurret;

    public TextMeshPro TeamNameText, ProducedBulletText;
    
    public ProducedBullet ProducedBulletPrefab;

    public void SetCanShoot(bool _canShoot)
    {
        canShoot = _canShoot;
    }
    
    [SerializeField] private bool canShoot;

    [SerializeField] private int bulletSupply;

    private float playTime;

    private void Update()
    {
        if (!canShoot)
            return;

        if (bulletSupply <= 0)
            return;

        playTime += Time.deltaTime;
        if (playTime >= GameManager.instance.gameSettings.shootInterval)
        {
            playTime = 0;
            TeamTurret.ShootBullet();
            bulletSupply -= 1;
            ProducedBulletText.text = bulletSupply.ToString();
        }

    }

    public void InitiateTeam(int teamId, GameSettings.Team team)
    {
        BulletProducerTeamId = teamId;
        BulletProducerColor = team.TeamColor;

        TeamNameText.text = team.TeamName;
        TeamNameText.color = team.TeamColor;
        ProducedBulletText.color = team.TeamColor;
        
        TeamTurret.InitiateTeam(teamId, team);

        ProducedBulletText.text = "0";
    }
    
    

    public void ProduceBullet(int amount)
    {
        bulletSupply += amount;
        ProducedBulletText.text = bulletSupply.ToString();
        ProducedBullet newProduced = LeanPool.Spawn(ProducedBulletPrefab, BulletStartPoint.position, Quaternion.identity);
        newProduced.InitiateProducedBullet(BulletProducerColor);
        newProduced.transform.DOMoveZ(BulletEndPoint.position.z, 2).onComplete = () =>
        {
            newProduced.Despawn();
        };
        // Debug.Log("This Machine "+BulletProducerTeamId+" produced bullet", gameObject);
    }
    
}
