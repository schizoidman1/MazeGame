using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "MapGen/GameInfo", order = 1)]
public class ObjectInfo : ScriptableObject
{
    public Color color;
    public GameObject prefab;
}
