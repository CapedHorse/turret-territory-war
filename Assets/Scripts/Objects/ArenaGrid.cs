using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArenaGrid : MonoBehaviour
{
    public Transform GridTweenTransform;
    
    public MeshRenderer GridMesh;

    public MeshRenderer GridPhotoPlane;

    public BoxCollider GridCollider;

    public GameSettings.Team CurrentTeam;

    public float ColorTweenSpeed = 0.15f, MoveTweenSpeed = 0.1f;

    private bool IsTweening;
   
    public void Initiate()
    {
        GridPhotoPlane.material.mainTexture = null;
        GridPhotoPlane.material.color = Color.black;
        GridMesh.material.color = Color.black;

        GridPhotoPlane.material.DOColor(Color.white, ColorTweenSpeed);
        GridMesh.material.DOColor(Color.white, ColorTweenSpeed);
    }

    public void TestTexture()
    {
        GridPhotoPlane.material.mainTexture = GameManager.instance.gameSettings.Teams[0].TeamPhotoTexture;
    }

    //tween the grid mesh to up then down but can only once
    //After done, automatically switch team anim
    public void TweenGridMesh(bool switchingTeams)
    {
        if (IsTweening)
            return;

        IsTweening = true;
        
        GridMesh.transform.DOLocalMoveY(1, MoveTweenSpeed).onComplete = () =>
        {
            GridMesh.transform.DOLocalMoveY(0, MoveTweenSpeed).onComplete = () =>
            {
                IsTweening = false;
                if(switchingTeams) SwitchTeams();
            };
        };
    }

    //switch color and picture of the team
    public void SwitchTeams()
    {
        GridPhotoPlane.material.mainTexture = null;
        GridPhotoPlane.material.color = Color.white;
        GridMesh.material.color = Color.white;

        Sequence GridInitiateTween = DOTween.Sequence();

        GridInitiateTween.Append(GridPhotoPlane.material.DOColor(Color.white, ColorTweenSpeed));
        GridInitiateTween.Join(GridMesh.material.DOColor(Color.white, ColorTweenSpeed));

        GridInitiateTween.Play().onComplete = () =>
        {
            GridPhotoPlane.material.mainTexture = CurrentTeam.TeamPhotoTexture;
            GridPhotoPlane.material.DOColor(CurrentTeam.TeamColor, ColorTweenSpeed);
            GridMesh.material.DOColor(CurrentTeam.TeamColor, ColorTweenSpeed).onComplete = () => CurrentTeam = new GameSettings.Team();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ArenaGrid>())
            return;
        if (!other.CompareTag("Bullet"))
            return;
        if (other.GetComponent<Bullet>())
        { 
            if (CurrentTeam != null && other.GetComponent<Bullet>().BulletTeamId == CurrentTeam.teamId)
                return;
            CurrentTeam = GameManager.instance.gameSettings.Teams[other.GetComponent<Bullet>().BulletTeamId]; 
            if(IsTweening) TweenGridMesh(true);
            else SwitchTeams();
            
            other.GetComponent<Bullet>().Despawn();
        }
    }
}
