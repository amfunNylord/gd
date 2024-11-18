using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public Database m_data;
    public List<ItemInventory> m_items = new List<ItemInventory>();

    public GameObject m_gameObjectShow;

    public GameObject m_inventoryMainObject;
    public int m_maxCount; // количество позиций куда можно поставить

    public Camera m_cam;
    public EventSystem m_es;
    public int m_currentId;
    public ItemInventory m_currentItem;
    public RectTransform m_movingObject;
    public Vector3 m_offset;

    public GameObject m_background;

    public GameObject m_availableFurnitureObject;
    public List<ItemInventory> m_furnitureItems = new List<ItemInventory>();

    public bool m_isAvailableFurniture = false;

    public void Start()
    {
        if (m_items.Count == 0)
        {
            AddGraphics();
        }

        /*for (int i = 0; i < m_maxCount; i++ ) // тест, заполнить рандомные ячейки
        {
            AddItem(i, m_data.m_items[Random.Range(0, m_data.m_items.Count)], 1);
        }*/

        for (int i = 0; i < m_data.m_items.Count; i++) 
        {
            m_furnitureItems[i].m_id = m_data.m_items[i].m_id;
            m_furnitureItems[i].m_count = 1;
            m_furnitureItems[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[i].m_img;
            m_furnitureItems[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
        }

        UpdateInventory();
    }

    public void Update()
    {
        if (m_currentId != -1)
        {
            MoveObject();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_background.SetActive(!m_background.activeSelf);
            if (m_background.activeSelf)
            {
                UpdateInventory();
            }
        }
    }

    public void AddItem(int id, Item item, int count)
    {
        m_items[id].m_id = item.m_id;
        m_items[id].m_count = count;
        m_items[id].m_itemGameObject.GetComponent<Image>().sprite = item.m_img;
        if (count > 1 && item.m_id != 0) 
        {
            m_items[id].m_itemGameObject.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            m_items[id].m_itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        m_items[id].m_id = invItem.m_id;
        m_items[id].m_count = invItem.m_count;
        m_items[id].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[invItem.m_id].m_img;
        if (invItem.m_count > 1 && invItem.m_id != 0)
        {
            m_items[id].m_itemGameObject.GetComponentInChildren<Text>().text = invItem.m_count.ToString();
        }
        else
        {
            m_items[id].m_itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddGraphics()
    {
        for (int i = 0; i < m_data.m_items.Count; i++)
        {
            GameObject newItem = Instantiate(m_gameObjectShow, m_availableFurnitureObject.transform) as GameObject;
            newItem.name = i.ToString() + "a";
            ItemInventory ii = new ItemInventory();
            ii.m_itemGameObject = newItem;
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });
            m_furnitureItems.Add(ii);
        }
        for (int i = 0; i < m_maxCount; ++i)
        {
            GameObject newItem = Instantiate(m_gameObjectShow, m_inventoryMainObject.transform) as GameObject;

            newItem.name = i.ToString() + "i";

            ItemInventory ii = new ItemInventory();
            ii.m_itemGameObject = newItem;
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            tempButton.onClick.AddListener(delegate { SelectObject(); });
            m_items.Add(ii);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < m_maxCount; ++i)
        {
            if (m_items[i].m_id != 0 && m_items[i].m_count > 1)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = m_items[i].m_count.ToString();
            }
            else
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
            }
            m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[m_items[i].m_id].m_img;
        }

        for (int i = 0; i < m_furnitureItems.Count; ++i)
        {
            if (m_data.m_items[i].m_id != 0 && m_furnitureItems[i].m_count > 1)
            {
                m_furnitureItems[i].m_itemGameObject.GetComponentInChildren<Text>().text = m_furnitureItems[i].m_count.ToString();
            }
            else
            {
                m_furnitureItems[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
            }
            m_furnitureItems[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[m_furnitureItems[i].m_id].m_img;
        }

    }

    // todo: смена когда меняешь в доступных местах
    /*public void SelectObject()
    {
        if (m_currentId == -1)
        {
            m_currentId = int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1));
            m_currentItem = CopyInventoryItem(m_items[m_currentId]);
            m_movingObject.gameObject.SetActive(true);
            m_movingObject.GetComponent<Image>().sprite = m_data.m_items[m_currentItem.m_id].m_img;

            AddItem(m_currentId, m_data.m_items[0], 0);
        }
        else
        {
            AddInventoryItem(m_currentId, m_items[int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1))]);
            AddInventoryItem(int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1)), m_currentItem);
          
            m_currentId = -1;

            m_movingObject.gameObject.SetActive(false);
        }
    }*/
    public void SelectObject()
    {
        if (m_currentId == -1)
        {
            if (m_es.currentSelectedGameObject.name[m_es.currentSelectedGameObject.name.Length - 1] == 'i')
            {
                m_currentId = int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1));
                m_currentItem = CopyInventoryItem(m_items[m_currentId]);
                m_movingObject.gameObject.SetActive(true);
                m_movingObject.GetComponent<Image>().sprite = m_data.m_items[m_currentItem.m_id].m_img;

                AddItem(m_currentId, m_data.m_items[0], 0);
            }
            else
            {
                m_currentId = int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1));
                m_currentItem = CopyInventoryItem(m_furnitureItems[m_currentId]);
                m_movingObject.gameObject.SetActive(true);
                m_movingObject.GetComponent<Image>().sprite = m_data.m_items[m_currentItem.m_id].m_img;
                m_isAvailableFurniture = true;
            }
        }
        else
        {
            if (m_es.currentSelectedGameObject.name[m_es.currentSelectedGameObject.name.Length - 1] == 'a')
            {
                return;
            }
            if (m_isAvailableFurniture)
            {
                AddInventoryItem(int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1)), m_currentItem);
                m_currentId = -1;

                m_movingObject.gameObject.SetActive(false);
                m_isAvailableFurniture = false;
            }
            else
            {
                AddInventoryItem(m_currentId, m_items[int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1))]);
                AddInventoryItem(int.Parse(m_es.currentSelectedGameObject.name.Substring(0, m_es.currentSelectedGameObject.name.Length - 1)), m_currentItem);

                m_currentId = -1;

                m_movingObject.gameObject.SetActive(false);
            }
        }
    }

    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + m_offset;
        pos.z = m_inventoryMainObject.GetComponent<RectTransform>().position.z;
        m_movingObject.position = m_cam.ScreenToWorldPoint(pos);

    }

    public ItemInventory CopyInventoryItem(ItemInventory old)
    {
        ItemInventory newItemInventory = new ItemInventory(); 

        newItemInventory.m_id = old.m_id;
        newItemInventory.m_itemGameObject = old.m_itemGameObject;
        newItemInventory.m_count = old.m_count;

        return newItemInventory;
    }
}

[System.Serializable]
public class ItemInventory
{
    public int m_id;
    public GameObject m_itemGameObject;

    public int m_count; // todo: delete
}
