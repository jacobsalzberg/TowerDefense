using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

    private TowerBtn towerBtnPresed;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        //going to look for spriterender in the towermanager
        spriteRenderer = GetComponent<SpriteRenderer>(); 
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

    public void PlaceTower (RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPresed !=null )
        {
            GameObject newTower = Instantiate(towerBtnPresed.TowerObject);
            newTower.transform.position = hit.transform.position;
            DisableDragSprite();
        }
    }
    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPresed = towerSelected;
        EnableDragSprite(towerBtnPresed.DragSprite);
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
    }
}
