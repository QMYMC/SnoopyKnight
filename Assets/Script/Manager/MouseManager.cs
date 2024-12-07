using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { };

public class MouseManager : Singleton<MouseManager>
{

    RaycastHit hitInfo;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    public Texture2D point, doorway, attack, target, arrow;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
       SetCursorTexture();
       MouseControl();
       
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(1) && hitInfo.collider != null) 
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
               OnMouseClicked?.Invoke(hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }

            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            } 

            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }


}
