using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GameStateControll : MonoBehaviour
{
    private bool _isActive  = false;
    public bool IsActive
    {
        get { return _isActive; } 

    }


    [SerializeField] private Sprite _spriteActive;
    [SerializeField] private Sprite _spriteStop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isActive = !_isActive;

            RenderSprite();
        }
    }

    private void RenderSprite()
    {
        if (_isActive)
        {
            GetComponent<Image>().sprite = _spriteActive;
        }
        else
        {
            GetComponent<Image>().sprite = _spriteStop;
        }
    }

    public void Stop()
    {
        _isActive = false;
        GetComponent<Image>().sprite = _spriteStop;
    }
}
