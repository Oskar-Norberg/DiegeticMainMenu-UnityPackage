using System.Collections.Generic;
using DiegeticMainMenu.Singleton;
using UnityEngine;

namespace DiegeticMainMenu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] private Menu startingMenu;

        public Menu CurrentMenu => _menuStack.Peek();

        public delegate void OnMenuChangedEventHandler();
        public event OnMenuChangedEventHandler OnMenuChanged;

        public delegate void OnMenuBackEventHandler();
        public event OnMenuBackEventHandler OnMenuBack;

        private Stack<Menu> _menuStack = new Stack<Menu>();

        protected override void Awake()
        {
            base.Awake();

            if (!startingMenu)
                Debug.LogWarning("No starting menu assigned to MenuManager");

            EnterSubMenu(startingMenu);
        }

        public void EnterSubMenu(Menu menu)
        {
            if (_menuStack.Count > 0)
                _menuStack.Peek()?.SetActive(false);

            _menuStack.Push(menu);
            menu.SetActive(true);

            OnMenuChanged?.Invoke();
        }

        public void Back()
        {
            if (_menuStack.Count <= 1)
                return;

            OnMenuBack?.Invoke();

            _menuStack.Pop().SetActive(false);
            _menuStack.Peek().SetActive(true);

            OnMenuChanged?.Invoke();
        }
    }
}