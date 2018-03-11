using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

    public TowerBtn towerBtnPresed { get; set; }
    private SpriteRenderer spriteRenderer;
    private List<Tower> TowerList = new List<Tower>();
    // list of build tiles!! remember what coliders we named
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;

	// Use this for initialization
	void Start () {
        //going to look for spriterender in the towermanager
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint,Vector2.zero);

            if (hit.collider.tag == "BuildSite")
            {
                hit.collider.tag = "BuildSiteFull";
                PlaceTower(hit);
            }           
        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }
    
    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSite()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildSite";
        }
        BuildList.Clear();
    }

    public void DestroyAllTower()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void PlaceTower (RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPresed !=null )
        {
            GameObject newTower = Instantiate(towerBtnPresed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPresed.TowerPrice);
            DisableDragSprite();
        }
    }

    public void BuyTower(int price)
    {
        GameManager.Instance.SubtractMoney(price);
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        //DO NOT ALLOW TO SELECT TOWER IF NO COINZ
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {         
            towerBtnPresed = towerSelected;
            EnableDragSprite(towerBtnPresed.DragSprite);
        }
        
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Make sure everything is on top of the page
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
        //towerBtnPresed = null;
    }
}
