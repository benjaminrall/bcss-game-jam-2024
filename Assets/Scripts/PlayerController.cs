using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerColour
{
    Red,
    Blue,
    Green,
    White,
}

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public SpriteRenderer playerSpriteRenderer;
    public Sprite[] sprites;
    public GameObject aim;
    public GameObject projectile;

    [Header("Movement Properties")]
    public float baseMoveSpeed = 2.0f;
    public float baseDashSpeed = 8.0f;
    public float baseDashDelay = 2.0f;
    
    [Header("Melee Attack Properties")]
    public float baseMeleeDamage = 1.0f;
    public float baseMeleeSpeed = 0.6f;
    public float baseMeleeRange = 1.0f;
    
    [Header("Ranged Attack Properties")]
    public float baseRangeDamage = 0.6f;
    public float baseRangeSpeed = 0.1f;
    public int baseMaxAmmo = 5;
    public float baseAmmoRechargeDelay = 1.2f;
    public float baseAmmoRechargeRate = 0.8f;

    [Header("Other Properties")] 
    public float baseHealth = 10.0f;
    public float baseDefence = 1.0f;

    // Player properties
    public float AmmoPercentage => _ammo / baseMaxAmmo;
    
    // Movement trackers
    private Vector2 _movement;
    private float _dashTimer;
    
    // Attack trackers
    private float _meleeAttackTimer;

    // Ammo trackers
    private float _ammo;
    private float _rangeAttackTimer;
    private float _ammoRechargeTimer;
    
    
    // Common relic-affected properties
    public float MoveSpeed { get; set; }
    public float DashSpeed { get; set; }
    public float DashDelay { get; set; }
    public float MeleeDamage { get; set; }
    public float MeleeSpeed { get; set; }
    public float MeleeRange { get; set; }
    public float RangeDamage { get; set; }
    public float RangeSpeed { get; set; }
    public float MaxAmmo { get; set; }
    public float AmmoRechargeDelay { get; set; }
    public float AmmoRechargeRate { get; set; }
    public float Health { get; set; }
    public float Defence { get; set; }
    
    // Uncommon relic powers
    public bool Combo { get; set; }
    public bool Stun { get; set; }
    public bool LifeSteal { get; set; }
    public bool Shotgun { get; set; }
    public bool Burst { get; set; }
    public bool Volatile { get; set; }
    public bool Homing { get; set; }
    public bool Momentum { get; set; }
    public bool Adrenaline { get; set; }
    public bool Resourceful { get; set; }
    public bool Phase { get; set; }
    public bool Toxic { get; set; }

    // Other
    private PlayerColour _colour;
    
    // Player components
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    /// Resets properties affected by relics to their base values.
    private void ResetRelicProperties()
    {
        // Resets all stats to their base values
        MoveSpeed = baseMoveSpeed;
        DashSpeed = baseDashSpeed;
        DashDelay = baseDashDelay;
        MeleeDamage = baseMeleeDamage;
        MeleeSpeed = baseMeleeSpeed;
        MeleeRange = baseMeleeRange;
        RangeDamage = baseRangeDamage;
        RangeSpeed = baseRangeSpeed;
        MaxAmmo = baseMaxAmmo;
        AmmoRechargeDelay = baseAmmoRechargeDelay;
        AmmoRechargeRate = baseAmmoRechargeRate;
        Health = baseHealth;
        Defence = baseDefence;
        
        // Resets all uncommon abilities
        Combo = false;
        Stun = false;
        LifeSteal = false;
        Shotgun = false;
        Burst = false;
        Volatile = false;
        Homing = false;
        Momentum = false;
        Adrenaline = false;
        Resourceful = false;
        Phase = false;
        Toxic = false;
    }
    
    /// Applies a list of relics to the player's stats.
    private void ApplyRelics(Relic[] relics)
    {
        ResetRelicProperties();
        
        foreach (var relic in relics)
        {
            // Checks that the relic applies to the player's current colour
            if (relic.Colour == _colour || relic.Colour == PlayerColour.White)
            {
                relic.ApplyEffect(this);
            }
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ResetRelicProperties();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        _ammo = MaxAmmo;
    }

    // Update is called once per frame
    private void Update()
    {
        // Handles player movement input
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        // Calculates angle towards the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMousePos = mousePos - _rigidbody.position;
        float angle = Mathf.Atan2(toMousePos.x, toMousePos.y) * Mathf.Rad2Deg;
        
        // Rotates aim to face towards the mouse
        aim.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        
        // Sets player rotational sprite
        int spriteIndex = Mathf.RoundToInt(angle / 45) % 8;
        if (spriteIndex < 0)
        {   
            spriteIndex += 8;
        }
        playerSpriteRenderer.sprite = sprites[spriteIndex];

        // Melee attacks
        if (_meleeAttackTimer > 0)
        {
            _meleeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("MeleeAttack");
            _meleeAttackTimer = MeleeSpeed;
        }
        
        // Ranged attacks
        if (_ammoRechargeTimer > 0)
        {
            _ammoRechargeTimer -= Time.deltaTime;
        } 
        else if (_ammo < MaxAmmo)
        {
            _ammo += Time.deltaTime * MaxAmmo / AmmoRechargeRate;
            
            if (_ammo > MaxAmmo)
            {
                _ammo = MaxAmmo;
            }
        }
        
        if (_rangeAttackTimer > 0)
        {
            _rangeAttackTimer -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire2") && _ammo >= 1)
        {
            _ammo -= 1;
            _rangeAttackTimer = RangeSpeed;
            
            // Shoot projectile
            GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
            p.GetComponent<ProjectileController>().Direction = toMousePos.normalized;
            
            _ammoRechargeTimer = AmmoRechargeDelay * (MaxAmmo - _ammo);
        }

        
    }

    private void FixedUpdate()
    {
        // Adjusts player velocity according to their movement input
        _rigidbody.linearVelocity = _movement * MoveSpeed;
    }
}
