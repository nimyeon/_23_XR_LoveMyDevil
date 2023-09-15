using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredPlatform : MonoBehaviour
{

    public float maxHeight = 1.0f; // 최대 높이
    private float initialYOffset = -0.5f; // 초기 Offset Y 값
    private float initialSizeY = 0.0f; // 초기 Y 축 크기
    private float ratio; //비율
    
    private BoxCollider2D _collider;
    private Vector3 _scale;
    private float max = 8;
    private float curfillingAmount = 0;
    [SerializeField] private Transform _mask;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _scale = transform.localScale;
        ratio = (0 - initialYOffset) / (1 - initialSizeY);
    }
    // Update is called once per frame
    private void Update()
    {
        OnSprayHit();
    }

    void OnSprayHit()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position,_scale,0);
        foreach(Collider2D i in hit)
        {
            if (i.CompareTag("Spray"))
            {
                i.GetComponent<CircleCollider2D>().enabled = false;
                PaintedPlatform();
            }
        }
    }

    void PaintedPlatform()
    {
        if(!_collider.enabled) 
            _collider.enabled = true;
        curfillingAmount += 0.1f;
        if (Mathf.Abs(curfillingAmount - max) <= 0.008f)
        {
            _collider.size = new Vector2(_collider.size.x, 1);
            _collider.offset = new Vector2(_collider.offset.x, 0);
            _mask.localPosition = new Vector3(_mask.localPosition.x,0);
            Destroy(this);
        }
        // 크기 조절
        float newSize = Mathf.Lerp(0, 1.0f, curfillingAmount/max);
        _collider.size = new Vector2(_collider.size.x, newSize);

        // Offset 계산
        float newOffset = initialYOffset + (ratio * (newSize - initialSizeY));
        newOffset = Mathf.Clamp(newOffset, initialYOffset, maxHeight);

        // Offset 값 설정
        _collider.offset = new Vector2(_collider.offset.x, newOffset);
        _mask.localPosition = new Vector2(_mask.localPosition.x, newSize-1);
    }
}
