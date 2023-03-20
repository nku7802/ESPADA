using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private Sprite newSprite;
        private static Tooltip instance;

        private SpriteRenderer spriteRenderer;
        
        private void Awake() 
        {
            instance = this;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Invoke("stagestart", 2.0f);  
            Sprite newSprite = Resources.Load<Sprite>($"UI/start")as Sprite;
            instance.spriteRenderer.sprite = newSprite;
        }

        private void stagestart()
        {
            instance.spriteRenderer.sprite = null;
        } 

}
