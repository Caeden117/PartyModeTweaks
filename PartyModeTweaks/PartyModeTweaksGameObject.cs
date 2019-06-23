using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PartyModeTweaks
{
    class PartyModeTweaksGameObject : MonoBehaviour
    {
        static PartyModeTweaksGameObject Instance;

        private Vector3 LeftScreenPosition;

        private bool setUp = false;
        private bool isEnabled = false;

        void Start()
        {
            if (Instance != null)
            {
                Instance.StartCoroutine(WaitForMainMenu());
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Instance.StartCoroutine(WaitForMainMenu());
        }

        void Update()
        {
            if (!setUp) return;
            GameObject[] filteredButtons = FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name == "BuyContainer" || g.name == "ClearLocalLeaderboardsButton").ToArray();
            foreach (GameObject button in filteredButtons) button.SetActive(!isEnabled);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleLeftScreen(isEnabled);
                StartCoroutine(WaitForLevelSelectionNavigationController(isEnabled));
                isEnabled = !isEnabled;
            }
        }

        private IEnumerator WaitForMainMenu()
        {
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<MainMenuViewController>().Any());
            MainMenuViewController mainmenu = Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault();
            if (mainmenu != null)
            {
                mainmenu.didFinishEvent -= MainMenuViewController_didFinishEvent;
                mainmenu.didFinishEvent += MainMenuViewController_didFinishEvent;
            }
        }

        private void MainMenuViewController_didFinishEvent(MainMenuViewController obj, MainMenuViewController.MenuButton buttonType)
        {
            if (buttonType == MainMenuViewController.MenuButton.Party)
            {
                GameObject leftScreen = GameObject.Find("LeftScreen");
                LeftScreenPosition = leftScreen.transform.position;
                leftScreen.transform.position -= new Vector3(0, 100, 0); //"If it works it's not stupid" ~Caeden117
                StartCoroutine(WaitForLevelSelectionNavigationController(false));
                setUp = true;
                isEnabled = true;
            }
        }

        private void ToggleLeftScreen(bool visible)
        {
            GameObject leftScreen = GameObject.Find("LeftScreen");
            if (visible) leftScreen.transform.position = LeftScreenPosition;
            else leftScreen.transform.position = new Vector3(0, -100, 0);
        }

        private IEnumerator WaitForLevelSelectionNavigationController(bool enabled)
        {
            yield return new WaitUntil(() => GameObject.Find("LevelSelectionNavigationController") != null);
            GameObject LevelSelection = GameObject.Find("LevelSelectionNavigationController");
            LevelSelection.transform.Find("BackArrowButton").gameObject.SetActive(enabled); //Yeet dat back button
        }
    }
}
