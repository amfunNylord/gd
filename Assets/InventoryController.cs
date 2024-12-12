using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    private ItemGrid selectedItemGrid;

    public ItemGrid SelectedItemGrid 
    { 
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }
    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                CreateRandomItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null) 
        {
            inventoryHighlight.Show(false);
            return; 
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) {  return; }

        selectedItem.Rotate();
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObjec(itemToInsert);

        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid)
        {
            return;
        }
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemToHighlight != null && itemToHighlight.itemData.name != "Empty")
            {
                inventoryHighlight.Show(true); 
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem; 

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    [SerializeField] ItemGrid startItemGrid;
    int spawnedItemsCount = 1; 
    public void AddItemToStartGrid()
    {
        if (spawnedItemsCount >= items.Count)
        {
            return;
        }

        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = spawnedItemsCount;
        inventoryItem.Set(items[selectedItemID]);


        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        Vector2Int? posOnGrid = startItemGrid.FindSpaceForObjec(itemToInsert);

        if (posOnGrid == null) { return; }

        startItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        spawnedItemsCount++;
    }

    [SerializeField] ItemGrid endItemGrid;
    public bool CheckWin()
    {
        bool res = true;
        Dictionary<ItemData, int> furnitureMap = new Dictionary<ItemData, int>();

        for (int i = 1; i < items.Count; ++i)
        {
            furnitureMap.Add(items[i], 0);
        }
        
        for (int i = 0; i < endItemGrid.inventoryItemSlot.GetLength(0); i++)
        {
            for (int j = 0; j < endItemGrid.inventoryItemSlot.GetLength(1); j++)
            {
                if (endItemGrid.inventoryItemSlot[i, j] != null)
                {
                    if (furnitureMap.ContainsKey(endItemGrid.inventoryItemSlot[i, j].itemData))
                    {
                        if (furnitureMap.TryGetValue(endItemGrid.inventoryItemSlot[i, j].itemData, out int value))
                        {
                            value++;
                            furnitureMap.Remove(endItemGrid.inventoryItemSlot[i, j].itemData);
                            furnitureMap.Add(endItemGrid.inventoryItemSlot[i, j].itemData, value);
                        }
                    }
                }
            }
        }

        foreach (var data in furnitureMap)
        {
            if (data.Value == 0)
            {
                res = false;
            }
        }

        return res;
    }

    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;

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

    public void LoadLevel2()
    {
        PlaceEmptyItem(0, 0);
        PlaceEmptyItem(1, 0);
        PlaceEmptyItem(2, 0); 
        PlaceEmptyItem(3, 0);

        PlaceEmptyItem(7, 0);
        PlaceEmptyItem(8, 0);
        PlaceEmptyItem(9, 0);
        PlaceEmptyItem(10, 0);

        PlaceEmptyItem(0, 1);
        PlaceEmptyItem(1, 1);

        PlaceEmptyItem(9, 1);
        PlaceEmptyItem(10, 1);

        PlaceEmptyItem(0, 2);
        PlaceEmptyItem(1, 2);

        PlaceEmptyItem(9, 2);
        PlaceEmptyItem(10, 2);

        PlaceEmptyItem(0, 3);
        PlaceEmptyItem(1, 3);
        PlaceEmptyItem(2, 3);
        PlaceEmptyItem(3, 3);

        PlaceEmptyItem(7, 3);
        PlaceEmptyItem(8, 3);
        PlaceEmptyItem(9, 3);
        PlaceEmptyItem(10, 3);
    }

    public void LoadLevel3()
    {
        PlaceEmptyItem(0, 0);
        PlaceEmptyItem(1, 0);
        PlaceEmptyItem(2, 0);
        PlaceEmptyItem(3, 0);
        PlaceEmptyItem(4, 0);
        PlaceEmptyItem(5, 0);

        PlaceEmptyItem(8, 0);
        PlaceEmptyItem(9, 0);
        PlaceEmptyItem(10, 0);

        PlaceEmptyItem(0, 1);
        PlaceEmptyItem(1, 1);
        PlaceEmptyItem(2, 1);
        PlaceEmptyItem(3, 1);
        PlaceEmptyItem(4, 1);

        PlaceEmptyItem(8, 1);
        PlaceEmptyItem(9, 1);
        PlaceEmptyItem(10, 1);

        PlaceEmptyItem(0, 2);
        PlaceEmptyItem(1, 2);
        PlaceEmptyItem(2, 2);
        PlaceEmptyItem(3, 2);

        PlaceEmptyItem(8, 2);
        PlaceEmptyItem(9, 2);
        PlaceEmptyItem(10, 2);

        PlaceEmptyItem(0, 3);
        PlaceEmptyItem(1, 3);
        PlaceEmptyItem(2, 3);

        PlaceEmptyItem(8, 3);
        PlaceEmptyItem(9, 3);
        PlaceEmptyItem(10, 3);
    }

    public void LoadLevel4()
    {
        PlaceEmptyItem(0, 0);
        PlaceEmptyItem(1, 0);
        PlaceEmptyItem(2, 0);
        PlaceEmptyItem(3, 0);
        PlaceEmptyItem(4, 0);
        PlaceEmptyItem(5, 0);
        PlaceEmptyItem(6, 0);
        PlaceEmptyItem(7, 0);
        PlaceEmptyItem(8, 0);
        PlaceEmptyItem(9, 0);
        PlaceEmptyItem(10, 0);

        PlaceEmptyItem(0, 1);
        PlaceEmptyItem(1, 1);
        
        PlaceEmptyItem(9, 1);
        PlaceEmptyItem(10, 1);

        PlaceEmptyItem(0, 2);
        PlaceEmptyItem(1, 2);

        PlaceEmptyItem(9, 2);
        PlaceEmptyItem(10, 2);

        PlaceEmptyItem(0, 3);
        PlaceEmptyItem(1, 3);
        PlaceEmptyItem(2, 3);
        PlaceEmptyItem(3, 3);

        PlaceEmptyItem(7, 3);
        PlaceEmptyItem(8, 3);
        PlaceEmptyItem(9, 3);
        PlaceEmptyItem(10, 3);
    }

    private void PlaceEmptyItem(int x, int y)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = 0;
        inventoryItem.Set(items[selectedItemID]);


        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        Vector2Int? posOnGrid = new Vector2Int(x, y);

        endItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
}
