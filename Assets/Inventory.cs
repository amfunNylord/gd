using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Schema;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public Database m_data;
    public List<ItemInventory> m_items = new List<ItemInventory>();

    public GameObject m_gameObjectShow;

    public GameObject m_inventoryMainObject;
    public int m_maxCount; // ���������� ������� ���� ����� ���������

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
    public GameObject winCanvas;
    public GameObject loseCanvas;

    public void Start()
    {
        if (m_items.Count == 0)
        {
            AddGraphics();
        }

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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < m_maxCount; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[1].m_img;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            for (int i = 0; i < m_maxCount; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[1].m_img;
            }

            for (int i = 0; i < 4; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 7; i < 11; ++i)
            {

                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 33; i < 37; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";   
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 11; i < 13; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 20; i < 22; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 22; i < 24; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 31; i < 33; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 40; i < 44; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            for (int i = 0; i < m_maxCount; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";  
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[1].m_img;
            }

            for (int i = 0; i < 6; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 8; i < 11; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 11; i < 16; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 19; i < 22; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 22; i < 26; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 30; i < 33; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
            for (int i = 33; i < 36; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
            for (int i = 41; i < 44 ; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            for (int i = 0; i < m_maxCount; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[1].m_img;
            }

            for (int i = 0; i < 13; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
            for (int i = 20; i < 24; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
            for (int i = 31; i < 37; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }

            for (int i = 40; i < 44; ++i)
            {
                m_items[i].m_itemGameObject.GetComponentInChildren<Text>().text = "";
                m_items[i].m_itemGameObject.GetComponent<Image>().sprite = m_data.m_items[0].m_img;
            }
        }
    }

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

    public bool CheckWin()
    {
        bool res = true;
        Dictionary<int, int> furnitureMap = new Dictionary<int, int>();

        for (int i = 2; i < m_furnitureItems.Count; ++i)
        {
            furnitureMap.Add(m_furnitureItems[i].m_id, 0);
        }

        for (int i = 0; i < m_items.Count; ++i)
        {
            if (furnitureMap.ContainsKey(m_items[i].m_id))
            {
                if (furnitureMap.TryGetValue(m_items[i].m_id, out int value))
                {
                    value++;
                    furnitureMap.Remove(m_items[i].m_id);
                    furnitureMap.Add(m_items[i].m_id, value);
                }
            }
        }

        foreach (var data in furnitureMap)
        {
            if (data.Value != 1)
            {
                res = false;
            }
        }

        return res;
    }

    public void DoResult()
    {
        if (CheckWin())
        {
            winCanvas.SetActive(true);
        }
        else
        {
            loseCanvas.SetActive(true);
        }    
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

[System.Serializable]
public class ItemInventory
{
    public int m_id;
    public GameObject m_itemGameObject;

    public int m_count; // todo: delete
}
