using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public List<Item> m_items = new List<Item>();
}

[System.Serializable]

public class Item
{
    public int m_id;
    public string m_name;
    public Sprite m_img;
}
