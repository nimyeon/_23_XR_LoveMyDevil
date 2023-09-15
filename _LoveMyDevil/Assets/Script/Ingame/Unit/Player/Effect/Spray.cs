using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Spray : MonoBehaviour
{
    private CircleCollider2D my_collider;
    
    private bool isColiderCheck = false;
    protected SpriteRenderer _sprite;
    private Tween _tween;
    private int id;
    void Start()
    {
        my_collider = GetComponent<CircleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        AutoDestroy().Forget();
        id = Random.Range(0, 100);
        _tween = _sprite.DOColor(new Color(_sprite.color.r,_sprite.color.g,_sprite.color.b,0), 0.2f);
    
    }
    void Update()
    {
    }

    public void CancleDestroyCallback(Transform _transform)
    {
        transform.parent = _transform;
        isColiderCheck = true;
        Destroy(GetComponent<CircleCollider2D>());
    }
    private void OnDestroy()
    {
        isColiderCheck = true;
        _tween.Complete();
    }
    async UniTaskVoid AutoDestroy()
    {
        for (int i = 0; i < 15; i++)
        {
            if (isColiderCheck)
            {
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        Destroy(gameObject);
    }
    
}
