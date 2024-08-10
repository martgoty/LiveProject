using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CellsManager _cellsManager;
    [SerializeField] private MenuManager _menuManager;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _minZoom = 10f;
    [SerializeField] private float _maxZoom = 20f;

    private void Update()
    {

        if (!_menuManager.IsMenuOpen)
        {
            Move();
            Zoom();
        }

    }

    private void Zoom()
    {
        float scroolInput = Input.GetAxis("Mouse ScrollWheel");

        GetComponent<Camera>().orthographicSize -= scroolInput * _zoomSpeed * Time.deltaTime * 100;
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, _minZoom, _maxZoom);
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        transform.position += new Vector3(_moveSpeed * horizontal, _moveSpeed * vertical, 0);

        if(transform.position.x > _cellsManager.Width)
        {
            transform.position = new Vector3(_cellsManager.Width, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

        if(transform.position.y > _cellsManager.Height)
        {
            transform.position = new Vector3(transform.position.x, _cellsManager.Height, transform.position.z);
        }
        else if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
