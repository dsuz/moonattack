using UnityEngine;
// Photon 用の名前空間を参照する
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    /// <summary>動く力</summary>
    [SerializeField] float m_movePower = 10f;
    /// <summary>弾を発射する場所</summary>
    [SerializeField] Transform m_muzzle = null;
    /// <summary>弾のプレハブ名</summary>
    [SerializeField] string m_bulletResourceName = "PrefabResourceName";
    Rigidbody2D m_rb = null;
    PhotonView m_view = null;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!m_view || !m_view.IsMine) return;      // 自分が生成したものだけ処理する

        Move();
        Rotate();

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    void Fire()
    {
        PhotonNetwork.Instantiate(m_bulletResourceName, m_muzzle.position, m_muzzle.rotation);
    }

    /// <summary>
    /// マウスの方向を向く
    /// </summary>
    void Rotate()
    {
        Vector3 mousePositionInWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositionInWorldSpace.z = this.transform.position.z;
        this.transform.up = mousePositionInWorldSpace - this.transform.position;
    }

    /// <summary>
    /// 上下左右にキャラクターを動かす
    /// </summary>
    void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector2 dir = (Vector2.up * v + Vector2.right * h).normalized;

        if (dir != Vector2.zero)
        {
            m_rb.AddForce(dir * m_movePower, ForceMode2D.Force);
        }
    }
}
