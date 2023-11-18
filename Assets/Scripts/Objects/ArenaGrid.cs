using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ArenaGrid : MonoBehaviour
{
    public Transform GridTweenTransform;
    
    public MeshRenderer GridMesh;

    public MeshRenderer GridPhotoPlane;

    public GameSettings.Team CurrentTeam;

    public float ColorTweenSpeed = 0.15f;
    [FormerlySerializedAs("MoveTweenSpeed")] public float MoveTweenDuration = 1f;

    private bool IsTweening;

    private int teamId = 0;
   
    public void Initiate()
    {
        GridPhotoPlane.material.mainTexture = null;
        GridPhotoPlane.material.color = Color.black;
        GridMesh.material.color = Color.black;

        GridPhotoPlane.material.DOColor( GameManager.instance.gameSettings.DefaultTeamColor, ColorTweenSpeed);
        GridMesh.material.DOColor( GameManager.instance.gameSettings.DefaultTeamDarkColor, ColorTweenSpeed);

        CurrentTeam = GameManager.instance.gameSettings.GetTeam(1);
        SwitchTeams();
    }

    /*public void TestTexture()
    {
        GridPhotoPlane.material.mainTexture = GameManager.instance.gameSettings.GetTeam(1).TeamPhotoTexture;
    }*/

    //tween the grid mesh to up then down but can only once
    //After done, automatically switch team anim
    public void TweenGridMesh(bool switchingTeams)
    {
        if (IsTweening)
            return;

        DOTween.Complete(GridMesh.transform);

        IsTweening = true;

        GridMesh.transform.DOLocalJump(GridMesh.transform.localPosition
            /*new Vector3(GridMesh.transform.localPosition.x, 1, GridMesh.transform.localPosition.z)*/,
            1, 1, MoveTweenDuration).onComplete = () =>
        {
            if (switchingTeams) SwitchTeams();
            IsTweening = false;
        };

        /*.DOLocalMoveY(1, MoveTweenDuration).onComplete = () =>
    {
        GridMesh.transform.DOLocalMoveY(0, MoveTweenDuration).onComplete = () =>
        {
            

        };

    };*/
    }

    //switch color and picture of the team
    public void SwitchTeams()
    {
        /*GridPhotoPlane.material.mainTexture = null;
        GridPhotoPlane.material.color = GameManager.instance.gameSettings.DefaultTeamColor;
        GridMesh.material.color =  GameManager.instance.gameSettings.DefaultTeamDarkColor;*/

        Sequence GridInitiateTween = DOTween.Sequence();

        GridInitiateTween.Append(GridPhotoPlane.material.DOColor(Color.white, ColorTweenSpeed));
        GridInitiateTween.Join(GridMesh.material.DOColor(Color.white, ColorTweenSpeed));

        GridInitiateTween.Play().onComplete = () =>
        {
            GridPhotoPlane.material.mainTexture = CurrentTeam.TeamPhotoTexture;
            GridPhotoPlane.material.DOColor(CurrentTeam.TeamColor, ColorTweenSpeed);
            GridMesh.material.DOColor(CurrentTeam.TeamDarkColor, ColorTweenSpeed);
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
            if (other.GetComponent<Bullet>().BulletTeamId == teamId)
                return;
            
            CurrentTeam = GameManager.instance.gameSettings.GetTeam(other.GetComponent<Bullet>().BulletTeamId);
            teamId = CurrentTeam.teamId;
            TweenGridMesh(true);
            other.GetComponent<Bullet>().Despawn();
            
           
            
        }
    }
}
