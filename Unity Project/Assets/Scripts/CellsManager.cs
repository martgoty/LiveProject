using UnityEngine;
using TMPro;
using System.Collections;

public class CellsManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private TextMeshProUGUI _genTxt;
    [SerializeField] private GameStateControll _gameState;
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private float _liveSpeed = 0.5f;
    public float LiveSpeed
    {
        get { return _liveSpeed; }
        set { _liveSpeed = value; }
    }

    private Cell[,] _cells;

    private int _generation = 0;
    private bool _canContinue = true;        
    private bool _isLeftMouseDown = false;
    private bool _isRightMouseDown = false;
    private int _width = 10;
    private int _heigth = 10;
    public int Width
    {
        get { return _width; }
    }
    public int Height
    {
        get { return _heigth; }
    }


    private void Start()
    {
        _genTxt.text = "0";
    }
    
    private void ResetCells()
    {
        _generation = 0;
        _genTxt.text = "0";
        foreach (Cell cell in _cells) {
            cell.CellDead();
        }
        _gameState.Stop();
    }
    private void CellLiveCheck()
    {
        int[,] newCells = new int[_width, _heigth];
        int liveNeighbours = 0;

        for (int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _heigth; y++)
            {
                //Поиск "живых" соседей
                for(int x0 = IndexClamp(x - 1, _width); x0 <= IndexClamp(x + 1, _width); x0++)
                {
                    for (int y0 = IndexClamp(y - 1, _heigth); y0 <= IndexClamp(y + 1, _heigth); y0++)
                    {
                        if(x0 == x && y0 == y)
                        {
                            continue;
                        }

                        if (_cells[x0, y0].IsAlive)
                        {
                            liveNeighbours++;
                        }
                    }
                }

                if (_cells[x, y].IsAlive)
                {
                    if(liveNeighbours < 2 || liveNeighbours > 3)
                    {
                        newCells[x, y] = 1;
                    }
                }
                else
                {
                    if(liveNeighbours == 3)
                    {
                        newCells[x, y] = 1;
                    }
                }

                liveNeighbours = 0;
                
            }

        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _heigth; y++)
            {
                if (newCells[x,y] == 1)
                {
                    _cells[x, y].ToogleState();
                }

            }

        }
    }

    public void GenerateCells(int width, int heigth)
    {

        _width = width; 
        _heigth = heigth;
        _cells = new Cell[_width, _heigth];

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _heigth; y++) {
                Vector3 pos = new Vector3(x, y, 0);

                GameObject cellGameObject = Instantiate(_cellPrefab, pos, Quaternion.identity);
                Cell cell = cellGameObject.GetComponent<Cell>();
                _cells[x ,y] = cell;
            }
        }
       Camera.main.transform.position = new Vector3(_width / 2, _heigth / 2, Camera.main.transform.position.z);


    }

    private void Drawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isLeftMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isLeftMouseDown = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _isRightMouseDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isRightMouseDown = false;
        }

        if (_isLeftMouseDown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.FloorToInt(Mathf.Round(mousePos.x));
            int y = Mathf.FloorToInt(Mathf.Round(mousePos.y));

            if (x >= 0 && x < _width && y >= 0 && y < _heigth)
            {
                _cells[x, y].CellLive();
            }
        }
        if (_isRightMouseDown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.FloorToInt(Mathf.Round(mousePos.x));
            int y = Mathf.FloorToInt(Mathf.Round(mousePos.y));

            if (x >= 0 && x < _width && y >= 0 && y < _heigth)
            {
                _cells[x, y].CellDead();
            }
        }
    }

    private void Update()
    {
        if (!_menuManager.IsMenuOpen) {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCells();
            }

            Drawing();

            if (_gameState.IsActive && _canContinue)
            {
                StartCoroutine(LiveTick());
                CellLiveCheck();
                _generation++;
                _genTxt.text = _generation.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                CellLiveCheck();
                _generation++;
                _genTxt.text = _generation.ToString();
            }
        }
    }

    private int IndexClamp(int index, int max)
    {
        if(index >= 0)
        {
            if(index < max)
            {
                return index;
            }
            else
            {
                return max - 1;
            }
        }
        else
        {
            return 0;
        }
       
    }

    IEnumerator LiveTick()
    {
        _canContinue = false;
        yield return new WaitForSeconds(_liveSpeed);
        _canContinue = true;
    }
}
