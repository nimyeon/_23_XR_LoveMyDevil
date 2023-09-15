using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 컴포넌트들
    public Rigidbody2D _playerRigidbody { get; private set; }
    private PlayerContrl _playerControll;

    //플레이어 스탯 값
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 1000;

    /// <summary>
    /// 최대 점프 횟수
    /// </summary>
    [SerializeField] private int MAXJUMP = 2;

    //기타 트리거들
    private bool _isjumping;
    private int jumpCount = 0;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerControll = GetComponent<PlayerContrl>();

    }

    void Update()
    {
        Jump();
        transform.Translate(new Vector3(speed * _playerControll.Userinput.AxisState, 0) * Time.deltaTime);
    }

    void Jump()
    {
        if (jumpCount >= MAXJUMP) return;
        if (!_playerControll.Userinput.SpaceState) return;
        _isjumping = true;
        jumpCount++;

        if (jumpCount == 2)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            return;
        }
        _playerRigidbody.AddForce(new Vector2(0, jumpForce));
        _playerRigidbody.velocity = Vector2.zero;


    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Ground")||
            other.gameObject.CompareTag("ColoredPlatform")||
            other.gameObject.CompareTag("DropPlatform")) && other.contacts[1].normal.y > 0.7f)
        {
            _isjumping = false;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Ground")||
            other.gameObject.CompareTag("ColoredPlatform") ||
            other.gameObject.CompareTag("DropPlatform")) && !_isjumping)
        {
            _isjumping = true;
            jumpCount = 1;
        }
        if (other.transform.CompareTag("DropPlatform") && other.transform.position.y < transform.position.y)
        {
            other.transform.GetComponent<DroppedPlatform>().Dropped().Forget();
        }
    

    }




}
