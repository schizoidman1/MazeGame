using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public Texture2D texture;
    public ObjectInfo[] objectInfo;
    public Vector3 offset = Vector3.zero;

    private Vector2 pos;
    void Start()
    {
        this.ReadTexture();
    }

    private void ReadTexture()
    {
        for(int x = 0; x < this.texture.width; x++) 
        {
            for (int y = 0; y < this.texture.height; y++)
            {
                this.pos = new Vector2(x, y);
                this.GetColor(x, y);
            }
        }
    }

    private void GetColor(int x, int y)
    {
        Color c = this.texture.GetPixel(x, y);
        if (c.a < 1)
            return;

        this.CreateObject(c);
    }

    private void CreateObject(Color c)
    {
        foreach (ObjectInfo info in objectInfo)
        {
            if (info.color == c)
            {
                GameObject obj = Instantiate(info.prefab, new Vector3(this.pos.x, 0, this.pos.y) + offset, Quaternion.identity, this.transform);
                
                // Se o objeto instanciado for a câmera, define a posição desejada
                if (obj.CompareTag("MainCamera"))
                {
                    obj.transform.position = new Vector3(5.38f, 13.79f, 1.84f); // Defina as coordenadas desejadas aqui
                    obj.transform.rotation = Quaternion.Euler(90f, 0, 0);
                }

                if (obj.CompareTag("Enemy"))
                {
                    Debug.Log("Inimigo instanciado.");
                }
            }
        }
    }
}
