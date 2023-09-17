using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMonster : MonoBehaviour
{
    [SerializeField] ColliderCallbackController colliderCallbackController;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float _speed = 3;
    private Rigidbody2D _rigid;
    
    private bool _targetedPlayer;
    private Transform player;
    int nextMove;
    private bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        MoveSelect().Forget();
    }
    
    private void OnEnable()
    {
        colliderCallbackController.onColiderEnter += Findedplayer;
    }
    private void OnDestroy()
    {
        isDie = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_targetedPlayer)
        {
            NontargetPlayerMove();
        }
        else
        {
            TargetedPlayer();
        }
    }

    private Vector3 toPos;
    void TargetedPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) >= distanceFromPlayer)
        {
            toPos = (player.position - transform.position).normalized;
            transform.Translate(toPos * (_speed * Time.deltaTime));
        }
        if (player.transform.position.x < transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        var frontVec = new Vector2(_rigid.position.x + (transform.localScale.x > 0 ? -0.5f : 0.5f),
            _rigid.position.y + 0.2f);
        Debug.DrawLine(frontVec, frontVec + new Vector2(0, 1), Color.red);
    }

    void NontargetPlayerMove()
    {
        _rigid.velocity = new Vector2(nextMove, 0);
    }
    
    
    async UniTaskVoid MoveSelect()
    {
        while (!isDie)
        {
            nextMove =  Random.Range(-1, 2);
            transform.localScale = nextMove > 0 ? new Vector3(-1,1) : new Vector3(1,1);
            await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);
            await UniTask.WaitUntil(() => !_targetedPlayer);
        }
    }
    protected void Findedplayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTargetPlayer(other.transform);
            
        }
    }
    protected virtual void SetTargetPlayer(Transform _player)
    {
        if (_targetedPlayer) return;
        
        player = _player;
        _targetedPlayer = true;
    }
    
}
