using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [SerializeField]
    private float _beHitInterval = .2f;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Animator _animator;

    private Color mDefaultColor;
    private Color mNormalColor;

    public void Walk()
    {
        _spriteRenderer.color = mDefaultColor;
        mNormalColor = mDefaultColor;
    }

    public void BeHit()
    {
        _animator?.SetTrigger("Hit");

        StartCoroutine(eBeHit());
    }

    public void Stun()
    {
        _spriteRenderer.color = Color.magenta;
        mNormalColor = Color.magenta;
    }

    private void Awake()
    {
        // _spriteRenderer = GetComponent<SpriteRenderer>();
        mDefaultColor = _spriteRenderer.color;
        mNormalColor = _spriteRenderer.color;
    }

    private IEnumerator eBeHit()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(_beHitInterval);

        _spriteRenderer.color = mNormalColor;
    }
}
