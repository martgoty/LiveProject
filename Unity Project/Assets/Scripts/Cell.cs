using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color _liveColor;
    [SerializeField] private Color _deadColor;

    private bool _isAlive = false;

    public bool IsAlive
    {
        get { return _isAlive; }
    }
  

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = _isAlive ? _liveColor : _deadColor;
    }
    public void CellLive()
    {
       _isAlive = true;
       GetComponent<SpriteRenderer>().color =  _liveColor;
    }

    public void CellDead()
    {
        _isAlive = false;
        GetComponent<SpriteRenderer>().color = _deadColor;
    }

    public void ToogleState()
    {
        _isAlive = !_isAlive;

        GetComponent<SpriteRenderer>().color = _isAlive ? _liveColor : _deadColor;

    }
}
