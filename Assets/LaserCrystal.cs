using UnityEngine;

public class LaserCrystal : MonoBehaviour, IEnemyBehaviour
{
    public float laserRange = 10.0f;
    public float shootSpeed = 0.5f;
    public float laserDamage = 1.0f;
    public float lineOfSightRadius = 0.2f;
    public float laserDuration = 0.5f;
    public float windUpDuration = 0.2f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;
    public LineRenderer laser;
    
    private float _shootTimer;
    private float _windUpTimer;
    private float _laserActiveTimer;
    private Vector2 _laserDirection;
    private bool _windingUp;
    private bool _damagedPlayer;
    
    private Transform _player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().transform;
        laser.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_laserActiveTimer > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _laserDirection, laserRange, playerLayer);

            if (!_damagedPlayer && hit.collider)
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                player.Damage(laserDamage);
                laser.SetPosition(1, hit.point);
                _damagedPlayer = true;
            }
            
            _laserActiveTimer -= Time.deltaTime;
        } 
        else if (laser.enabled)
        {
            laser.enabled = false;
        }
        
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        } 
        else if (Vector2.Distance(_player.position, transform.position) <= laserRange && LineOfSight())
        {
            _shootTimer = 1 / shootSpeed;
            _windUpTimer = windUpDuration;
            _windingUp = true;
            _laserDirection = (_player.position - transform.position).normalized;
        }

        if (_windUpTimer > 0)
        {
            _windUpTimer -= Time.deltaTime;
        } 
        else if (_windingUp)
        {
            _windingUp = false;
            _laserActiveTimer = laserDuration;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _laserDirection, laserRange, obstacleLayer);
            
            laser.SetPosition(0, transform.position);

            if (hit.collider)
            {
                laser.SetPosition(1, hit.point);
                laser.enabled = true;
            }
            else
            {
                laser.SetPosition(1, hit.point + _laserDirection * laserRange);
                laser.enabled = true;
            }
            
            _damagedPlayer = false;
        }
    }
    
    private bool LineOfSight()
    {
        Vector3 position = transform.position;
        
        Vector2 directionToPlayer = (_player.position - position);
        float distance = directionToPlayer.magnitude;
        RaycastHit2D hit = Physics2D.CircleCast(position, lineOfSightRadius, directionToPlayer.normalized, distance, obstacleLayer);

        return !hit.collider;
    }
    
    public Vector2 GetMovement()
    {
        return Vector2.zero;
    }

    public int GetSpriteIndex(int n)
    {
        Vector2 toPlayer = _player.position - transform.position;
        float angle = Mathf.Atan2(toPlayer.x, toPlayer.y) * Mathf.Rad2Deg;

        if (n == 6)
        {
            angle += 30;
        }
        
        int spriteIndex = Mathf.RoundToInt(n * angle / 360) % n;
        if (spriteIndex < 0) spriteIndex += n;
        
        return spriteIndex;
    }
}
