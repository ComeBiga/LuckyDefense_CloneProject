using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [SerializeField]
    private float _beHitInterval = .2f;

    private SpriteRenderer _spriteRenderer;
    private Color mDefaultColor;

    public void BeHit()
    {
        StartCoroutine(eBeHit());
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        mDefaultColor = _spriteRenderer.color;
    }

    private IEnumerator eBeHit()
    {
        _spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(_beHitInterval);

        _spriteRenderer.color = mDefaultColor;
    }
}
