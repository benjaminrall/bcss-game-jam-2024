using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public Sprite[] sprites;
    public GameObject aim;

    [Header("Movement")]
    public float moveSpeed = 2.0f;
    
    [Header("Attacks")]
    public float meleeAttackSpeed = 0.6f;
    public int maxAmmo = 5;
    public GameObject projectile;
    public float rangeAttackSpeed = 0.2f;
    public float ammoRechargeDelay = 2.0f;
    public float ammoRechargeRate = 1.0f;

    private Vector2 _movement;
    private float _meleeAttackTimer;

    private float _ammo;
    private float _rangeAttackTimer;
    private float _ammoRechargeTimer;
    public float AmmoPercentage => _ammo / maxAmmo;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _ammo = maxAmmo;
    }

    // Update is called once per frame
    private void Update()
    {
        // Handles player movement input
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        // Calculates angle towards the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMousePos = mousePos - _rb.position;
        float angle = Mathf.Atan2(toMousePos.x, toMousePos.y) * Mathf.Rad2Deg;
        
        // Rotates aim to face towards the mouse
        aim.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        
        // Sets player rotational sprite
        int spriteIndex = Mathf.RoundToInt(angle / 45) % 8;
        if (spriteIndex < 0)
        {   
            spriteIndex += 8;
        }
        _sr.sprite = sprites[spriteIndex];
        
        // Ensures player renders the correct side of attacks
        _sr.sortingOrder = angle is >= 90 or <= -90 ? -1 : 1;

        // Melee attacks
        if (_meleeAttackTimer > 0)
        {
            _meleeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("MeleeAttack");
            _meleeAttackTimer = meleeAttackSpeed;
        }
        
        // Ranged attacks
        if (_ammoRechargeTimer > 0)
        {
            _ammoRechargeTimer -= Time.deltaTime;
        } 
        else if (_ammo < maxAmmo)
        {
            _ammo += Time.deltaTime * maxAmmo / ammoRechargeRate;
            if (_ammo > maxAmmo)
            {
                _ammo = maxAmmo;
            }
        }
        
        if (_rangeAttackTimer > 0)
        {
            _rangeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire2") && _ammo >= 1)
        {
            _ammo -= 1;
            _rangeAttackTimer = rangeAttackSpeed;
            
            // Shoot projectile
            GameObject p = Instantiate(projectile);
            p.transform.position = transform.position;
            p.GetComponent<ProjectileController>().Direction = toMousePos.normalized;
            
            _ammoRechargeTimer = ammoRechargeDelay;
        }

        
    }

    private void FixedUpdate()
    {
        // Adjusts player velocity according to their movement input
        _rb.linearVelocity = _movement * moveSpeed;
    }
}
