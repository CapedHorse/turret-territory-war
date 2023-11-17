using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class ProducedBullet : MonoBehaviour
{
    public MeshRenderer ProducedMesh;
    // public TextMeshPro ProducedAmountText;

    public void InitiateProducedBullet(Color color)
    {
        ProducedMesh.material.color = color;
    }

    public void Despawn()
    {
        ProducedMesh.material.color = Color.white;
        LeanPool.Despawn(gameObject);
    }
}
