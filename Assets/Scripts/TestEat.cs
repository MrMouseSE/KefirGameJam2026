using UnityEngine;

public class TestEat : MonoBehaviour
{
    public float Speed = 2.0f;
    public float Distance = 3.0f;

    private float _targetY;
    private bool _shouldMove = false;
    
    
    public void MoveBug()
    {
        _targetY = transform.position.y - Distance;
        _shouldMove = true;
    }

    void Update()
    {
        if (!_shouldMove) return;
        
        
        var newY = Mathf.MoveTowards(transform.position.y, _targetY, Speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            
        if (Mathf.Approximately(transform.position.y, _targetY)) _shouldMove = false;
    }
}