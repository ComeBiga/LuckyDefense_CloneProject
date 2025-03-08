using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimation : MonoBehaviour
{
    [SerializeField]
    private float _attackInterval = .2f;
    [SerializeField]
    private Animator _animator;

    private SpriteRenderer _spriteRenderer;
    private Color mDefaultColor;

    public void Attack()
    {
        _animator?.SetTrigger("Attack");

        StartCoroutine(eAttack());
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        mDefaultColor = _spriteRenderer.color;
    }

    private IEnumerator eAttack()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(_attackInterval);

        _spriteRenderer.color = mDefaultColor;
    }
}
