using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageble, IMoveble
{
    [SerializeField] private Animator _animator;

    [Header("Player Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private CharProgressSO _progressSO;
    private CharCharacteristics _charData => _progressSO.CurrentLevelData(_level);

    private float _currentSpeed = 1f;
    private Attacker _attacker;
    private Vector3 _move;
    private Camera _camera;

    private int _currentHealth;
    private bool _alive = true;

    public float Speed => _currentSpeed;
    public float MaxSpeed => _charData.MaxSpeed;
    public float SpeedIncrase => _charData.SpeedIncrase;
    public int Health => _currentHealth;
    public int MaxHealth => _charData.MaxHP;

    public EventHandler<int> TakeDamage => OnTakeDmg;
    public EventHandler<int> TakeHeal => OnHeal;

    public static event Action<int, int> PlayerHealthChanged;

    
    

    private void Start()
    {
        _currentHealth = MaxHealth;
        _alive = _currentHealth > 0;
        PlayerHealthChanged?.Invoke(_currentHealth, MaxHealth);
    }

    private void Die()
    {
        print("Dead");
    }

    private void OnHeal(object sender, int heal)
    {
        if (_currentHealth < MaxHealth)
            _currentHealth += heal;

        if (_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;

        PlayerHealthChanged?.Invoke(_currentHealth, MaxHealth);
    }

    private void OnTakeDmg(object sender, int damage)
    {

        if (sender is not Attacker)
            return;

        if (_currentHealth > damage)
            _currentHealth -= damage;
        else if (_alive)
        {
            _alive = false;
            _currentHealth = 0;
            Die();
        }

        print($"Player Health: {_currentHealth} | Dmg: {damage}");
        PlayerHealthChanged?.Invoke(_currentHealth, MaxHealth);
    }

}
