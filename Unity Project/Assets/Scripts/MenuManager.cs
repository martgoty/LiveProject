using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _menu;
    [SerializeField] TMP_InputField _widthField;
    [SerializeField] TMP_InputField _heightField;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _sliderText;
    [SerializeField] CellsManager _cellsManager;


    private int _width = 0;
    private int _height = 0;
    private bool _isMenuOpen = false;
    private bool _isFirstStart = true;

    public bool IsMenuOpen
    {
        get { return _isMenuOpen; } 
    }

    private void Start()
    {
        _isMenuOpen = true;
        _widthField.text = "50";
        _heightField.text = "50";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_isMenuOpen) {
                if (!_isFirstStart)
                {
                    MenuSetInactive();
                }
            }
            else
            {
                MenuSetActive();
            }
        }
    }

    private void OnEnable()
    {
        _widthField.onValueChanged.AddListener(ValidateWidthInput);
        _heightField.onValueChanged.AddListener(ValidateHeightInput);
    }
    private void OnDisable()
    {
        _widthField.onValueChanged.RemoveAllListeners();
        _heightField.onValueChanged.RemoveAllListeners();
    }

    private void MenuSetActive()
    {
        _isMenuOpen = true;
        _UI.SetActive(false);
        _menu.SetActive(true);
     
    }
    public void StartGame()
    {
        if(_width > 0 && _height > 0)
        {
            _isMenuOpen = false;
            _UI.SetActive(true);
            _menu.SetActive(false);

            _cellsManager.GenerateCells(_width, _height);
            _isFirstStart = false;
        }
    }
    private void MenuSetInactive()
    {
        _isMenuOpen = false;
        _UI.SetActive(true);
        _menu.SetActive(false);
        Debug.Log(_isMenuOpen);


    }

    public void CloseApp()
    {
        Application.Quit();
    }

    void ValidateWidthInput(string input)
    {
        if (!int.TryParse(input, out int result) || result < 0)
        {
            _widthField.text = ""; 
        }
        else
        {
            _width = int.Parse(input);
        }
    }
    void ValidateHeightInput(string input)
    {
        if (!int.TryParse(input, out int result) || result < 0)
        {
            _heightField.text = "";
        }
        else
        {
            _height = int.Parse(input);
        }
    }

    public void OnSliderChange()
    {
        float temp = (21f - _slider.value) / 100;
        _cellsManager.LiveSpeed = temp;
        _sliderText.text = "x" + _slider.value.ToString();
    }
}



