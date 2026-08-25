using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassMenuManager : MonoBehaviour
{
    //Reference Variables
    public GameObject mathUpButton;
    public GameObject socialStudiesUpButton;
    public GameObject scienceUpButton;
    public GameObject englishUpButton;
    public GameObject socialPointsTextBox;


    public void Update()
    {
        socialPointsTextBox.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.Instance.currSocialPoints.ToString();
    }

    public void ExitClasses()
    {
        SceneManager.LoadScene("PartyRoom2D");
    }

    public void MathUp()
    {
        GameManager.Instance.currSocialPoints -= 1;
        GameManager.Instance.mathValue += 1;
    }

    public void SocialStudiesUp()
    {
        GameManager.Instance.currSocialPoints -= 1;
        GameManager.Instance.socialStudiesValue += 1;
    }

    public void ScienceUp()
    {
        GameManager.Instance.currSocialPoints -= 1;
        GameManager.Instance.scienceValue += 1;
    }

    public void EnglishUp()
    {
        GameManager.Instance.currSocialPoints -= 1;
        GameManager.Instance.englishValue += 1;
    }

    public void ActivateMath()
    {
        mathUpButton.SetActive(true);
        socialStudiesUpButton.SetActive(false);
        scienceUpButton.SetActive(false);
        englishUpButton.SetActive(false);
    }

    public void ActivateSocialStudies()
    {
        mathUpButton.SetActive(false);
        socialStudiesUpButton.SetActive(true);
        scienceUpButton.SetActive(false);
        englishUpButton.SetActive(false);
    }

    public void ActivateScience()
    {
        mathUpButton.SetActive(false);
        socialStudiesUpButton.SetActive(false);
        scienceUpButton.SetActive(true);
        englishUpButton.SetActive(false);
    }

    public void ActivateEnglish()
    {
        mathUpButton.SetActive(false);
        socialStudiesUpButton.SetActive(false);
        scienceUpButton.SetActive(false);
        englishUpButton.SetActive(true);
    }

}
