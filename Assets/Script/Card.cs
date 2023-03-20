using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardShape // 자료형의 자료를 만들었음
{
    Diamond=1, Club, Heart, Spade
}

public class Card : MonoBehaviour
{
    public CardShape Shape => shape;
    [SerializeField]
    public CardShape shape = CardShape.Diamond; // 카드 문양p

    public int Number => number;
    [SerializeField]
    public int number = 1; // 카드 번호
    [SerializeField]
    public bool flip = false; // 뒷면이 false
    [SerializeField]
    public bool selection = true; // 바꿀 수 없으면 false
    [SerializeField]
    private bool isMoving = false;

    private Vector3 originalPotision;

    public SpriteRenderer spriteRenderer; // sprite 변경
    new Renderer renderer;


    private void Awake() 
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();
        renderer = GetComponent<Renderer>();
    }
    
    public void Init(CardShape shape, int number) // 카드 모양 넘버 설정 및 sprite 변경
    {
        this.shape = shape;
        this.number = number;
        this.gameObject.name = $"Card_{shape.ToString()}_{number}";

        spriteRenderer = GetComponent<SpriteRenderer>(); // 카드 sprite 설정
        string spriteName = $"{this.number}_of_{this.shape.ToString().ToLower()}s";
        Sprite newSprite = Resources.Load<Sprite>($"Cards/{spriteName}");

        string materialName = $"{this.number}_of_{this.shape.ToString().ToLower()}s";
        Material newMaterial = Resources.Load<Material>($"Shader/{materialName}");

        if (newSprite == null)
        {
            Debug.LogError($"Sprite '{spriteName}' not found!");
        }
        else
        {
            spriteRenderer.sprite = newSprite;
            spriteRenderer.material = newMaterial;
        }

        Flip(false);
    }

    public void Move(Vector3 destination, bool setOriginalPosition=false) 
    {
        StartCoroutine(MoveRoutine(destination));
        this.originalPotision = destination;
    }

    private IEnumerator MoveRoutine(Vector3 destination) // 이동 애니메이션
    {
        isMoving = true;

        float duration = 1f;
        float timeElapsed = 0f;
        Vector3 startPosition = transform.position;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;

        isMoving = false;
    } 

     
    public void Flip(bool value)
    {
        flip = value;

        string spriteName = "0";
        string materialName = "0";
 
        if (flip == true) 
        {
            spriteName = $"{this.number}_of_{this.shape.ToString().ToLower()}s";
            materialName = $"{this.number}_of_{this.shape.ToString().ToLower()}s";
        }

        Sprite newSprite = Resources.Load<Sprite>($"Cards/{spriteName}");
        Material newMaterial = Resources.Load<Material>($"Shader/{materialName}");

        spriteRenderer.sprite = newSprite;
        spriteRenderer.material = newMaterial;
    }


    public void SetMaterial(Material material)
    {
        if (renderer != null)
        {
            renderer.material = material;
        }
        else
        {
            Debug.LogError("Renderer component not found on Card.");
        }
    }
    

    public bool IsSelect
    {
        get { return selection; }
    }
    public static int cardCheck = 0;

    public void OnMouseEnter() // 마우스 올렸을 때 행동
    {
        if(!flip) return;
        if(isMoving) return;

        spriteRenderer.enabled = false;
        Zoomer.SetSprite(spriteRenderer.sprite);
        Zoomer.SetMaterial(GetMaterial());
    }

    public void OnMouseExit() // 마우스가 나갔을 때 행동
    {
        transform.localScale = new Vector3(1, 1, 1);
        Zoomer.SetSprite(null);
        spriteRenderer.enabled = true;
        Zoomer.SetMaterial(Resources.Load<Material>("Shader/Sprite-Lit-Default"));
    }

    public Material GetMaterial() // Material 변경
    {
        if (renderer != null)
        {
            return renderer.material;
        }
        else
        {
            Debug.LogError("Renderer component not found on Card prefab.");
            return null;
        }
    }
}
