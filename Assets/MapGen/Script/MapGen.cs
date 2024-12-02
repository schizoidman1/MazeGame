using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI; // Importante para NavMeshSurface

public class MapGen : MonoBehaviour
{
    public Texture2D texture;
    public ObjectInfo[] objectInfo;
    public Vector3 offset = Vector3.zero;
    public NavMeshSurface navMeshSurface; // Referência ao NavMeshSurface no chão

    private Vector2 pos;

    void Start()
    {
        ReadTexture(); // Gera o mapa

        // Rebake do NavMesh após o mapa ser gerado
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh rebakeado com sucesso!");
        }
        else
        {
            Debug.LogError("NavMeshSurface não atribuído! Certifique-se de que está configurado no Inspector.");
        }
    }

    private void ReadTexture()
    {
        for (int x = 0; x < this.texture.width; x++)
        {
            for (int y = 0; y < this.texture.height; y++)
            {
                this.pos = new Vector2(x, y);
                GetColor(x, y);
            }
        }
    }

    private void GetColor(int x, int y)
    {
        Color c = this.texture.GetPixel(x, y);
        if (c.a < 1)
            return;

        CreateObject(c);
    }

    private void CreateObject(Color c)
    {
        foreach (ObjectInfo info in objectInfo)
        {
            if (info.color == c)
            {
                Vector3 spawnPosition = new Vector3(pos.x, 0, pos.y) + offset;

                if (info.prefab.CompareTag("Enemy"))
                {
                    // Verificar se a posição está no NavMesh
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(spawnPosition, out hit, 2.0f, NavMesh.AllAreas))
                    {
                        Instantiate(info.prefab, hit.position, Quaternion.identity, transform);
                        Debug.Log("Inimigo instanciado sobre o NavMesh.");
                    }
                    else
                    {
                        Debug.LogWarning($"Falha ao instanciar inimigo: posição ({spawnPosition}) fora do NavMesh.");
                    }
                }
                else
                {
                    Instantiate(info.prefab, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
    }
}
