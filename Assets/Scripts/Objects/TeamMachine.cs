using System;
using System.Collections;
using System.Collections.Generic;
using CapedHorse;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TeamMachine : MonoBehaviour, ITeamHolder
{
    public int BulletProducerTeamId;
    
    public Transform BulletStartPoint, BulletEndPoint;

    public Turret TeamTurret;

    public TextMeshPro TeamNameText, ProducedBulletText;

    public MeshRenderer[] MachineTrayMeshes;

    public ProducedBullet ProducedBulletPrefab;

    void SetCanShoot(bool _canShoot)
    {
        canShoot = _canShoot;
    }
    
    private bool canShoot;

    private int bulletSupply;

    private float playTime;

    private void Start()
    {
        
        GameManager.instance.OnGameStarted.AddListener(() =>
        {
            canShoot = true;
        });
    }

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
        }

    }

    public void InitiateTeam(int teamId, GameSettings.Team team)
    {
        BulletProducerTeamId = teamId;
        
        foreach (var mesh in MachineTrayMeshes)
        {
            foreach (var material in mesh.materials)
            {
                material.color = team.TeamColor;
            }
        }

        TeamNameText.text = team.TeamName;
        TeamNameText.color = team.TeamColor;
        ProducedBulletText.color = team.TeamColor;
        
        TeamTurret.InitiateTeam(teamId, team);
    }
    
    

    public void ProduceBullet(int amount)
    {
        bulletSupply += amount;
    }
    
}
