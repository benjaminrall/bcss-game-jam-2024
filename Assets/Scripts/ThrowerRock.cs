using System;
using Pathfinding;
using UnityEngine;

public class ThrowerRock : MonoBehaviour, IEnemyBehaviour
{
    public float moveSpeed = 4.0f;
    public float nextWaypointDistance = 1.0f;
    public float targetDistance = 2.0f;
    public float targetDistanceThreshold = 0.1f;
    public float lineOfSightRadius = 1.0f;
    public LayerMask obstacleLayer;
    public GameObject rockProjectile;
    public float throwSpeed;
    public float rockDamage;
    
    private float _throwTimer;

    private Seeker _seeker;
    private Path _path;
    private int _currentWaypoint;
    private Vector2 _previousMovement;

    private Transform _player;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().transform;
        _seeker = GetComponent<Seeker>();
        InvokeRepeating(nameof(UpdatePath), 0f, 1.0f);
    }

    private void Update()
    {
        if (_throwTimer > 0)
        {
            _throwTimer -= Time.deltaTime;
        } else if (Vector2.Distance(_player.position, transform.position) <= targetDistance && LineOfSight())
        {
            _throwTimer = 1 / throwSpeed;

            RockProjectileController p = Instantiate(rockProjectile, transform.position, Quaternion.identity)
                .GetComponent<RockProjectileController>();
            
            p.SetDamage(rockDamage);
            p.SetDirection((_player.position - transform.position).normalized);
        }
    }

    // Update is called once per frame
    private void UpdatePath()
    {
        if (_seeker.IsDone())
        {
            _seeker.StartPath(transform.position, _player.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        if (path.error) return;
        
        _path = path;
        _currentWaypoint = 0;
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
        float distance = Vector2.Distance(transform.position, _player.position);
        if (distance > targetDistance - targetDistanceThreshold && distance < targetDistance && LineOfSight())
        {
            return Vector2.zero;
        }
        
        if (_path == null || _currentWaypoint + 1 >= _path.vectorPath.Count) 
            return _previousMovement;
        if (Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]) < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
        Vector2 direction = _path.vectorPath[_currentWaypoint] - transform.position;
        


        if (distance < targetDistance)
        {
            direction *= -1;
        }

        _previousMovement = moveSpeed * direction.normalized;
        return  _previousMovement;
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
