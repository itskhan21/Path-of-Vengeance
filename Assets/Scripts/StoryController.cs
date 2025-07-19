using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    [SerializeField] GameObject textBox, father, varian, creditsScreen, creditsText;
    [SerializeField] Text dialogueBox;
    int currentPosition = 0, storyPhase = 0;
    string[] prologue = {
        "You are Prince Varian, the rightful heir to the throne and son of King Edwyn.",
        "Your uncle, driven by a lust for power, made a pact with a powerful demon, assassinating the king and framing Varian with fabricated evidence.",
        "Branded a traitor, you narrowly escape with your life, evading certain death at the hands of those whom you once trusted.",
        "As you hide and train in the isolated woods, the spirit of your father, King Edwyn begins to appear to you, offering guidance, courage, and the strength to push on.",
        "Fueled by grief, vengeance, and your father’s enduring wisdom, you prepare yourself to reclaim the throne, free your people, and restore the kingdom from the depths of darkness.",
        "While sharpening your skills in the depths of the Weeping Woods, a sudden ambush interrupts your focus.",
        "Your uncle's ruthless men have tracked you down, their mission is clear: eliminate you at all costs.",
        "Gear up, stay sharp, and fight your way through the treacherous forest.",
        "Show no mercy. It's you or them. Survive the onslaught and leave no enemy standing!"
    };

    string[] level_1_end = {
        "Father:\nVarian, my son, listen closely. Your uncle... he is no longer the man you remember. The demon he once foolishly made a pact with has taken full control of his body.",
        "Father:\nThat same demon now sits on the throne of Fendralia, ruling over the kingdom with cruelty and despair.",
        "Father:\nOur homeland, once a beacon of peace and harmony, has become a land of nightmares. The people cry out for salvation, but their voices are drowned in fear.",
        "Father:\nEvil festers in every corner, spreading like a plague under the grip of this demonic king.",
        "Father:\nVarian, you are the kingdom’s last hope. You must return to Fendralia, confront this darkness, and defeat the demon that has corrupted your uncle.",
        "Father:\nThis is not just about reclaiming the throne—it is about freeing your uncle’s soul from the torment that binds him.",
        "Father:\nI know this path is filled with danger, but you carry the strength of our lineage and the courage of a true hero.",
        "Father:\nGo, my son. Bring light back to Fendralia, and restore hope to its people. The kingdom’s fate rests in your hands."
    };

    string[] level_2_end =
    {
        "Father:\nVarian, you have saved the country, and your courage has inspired its people. But your duties are far from over. The fearsome devil, the root of this nightmare, Uhaynaten still remains.",
        "Father:\nHe lurks in the depths of the castle, controlling everything through your uncle’s possessed body. To truly free Fendralia, you must face him and put an end to this darkness once and for all.",
        "Father:\nGo now, Varian. Head to the castle and confront the demon. Only then will this nightmare end. The kingdom believes in you, my son, I believe in you. Finish what you have started."
    };

    string[] level_3_end =
    {
        "Father:\nVarian, you have done it. Uhaynaten the Devil is defeated, and the darkness that plagued Fendralia is no more. But, my son, I must tell you... there was no saving your uncle. His fate was sealed the moment he chose the demon's path.",

        "Father:\nHe chose his path, and you chose yours. You stood for what was right, and he has paid the ultimate price for his sins. Let his story serve as a warning to all who might seek power at the cost of their soul.",

        "Father:\nVarian, you are the new King of Fendralia. The throne is yours, and with it, the burden of leadership.",

        "Father:\nRule your kingdom with wisdom, honor, and compassion. Be the light that guides your people, the shield that protects them, and the strength they can rely on.",

        "Father:\nThis is your legacy, Varian. Wear your crown not as a symbol of power, but as a reminder of the responsibility you carry. Fendralia’s future lies in your hands. Lead it into an era of peace and prosperity."
    };


    GameManager gameManager;
    float fadeSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        gameManager.StoryPause();
        dialogueBox.text = prologue[currentPosition];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextButton()
    {
        if (storyPhase == 0)
        {
            if (currentPosition < prologue.Length - 1)
                dialogueBox.text = prologue[++currentPosition];
            else
            {
                currentPosition = 0;
                dialogueBox.text = level_1_end[currentPosition];
                gameManager.ResumeGame();
                textBox.SetActive(false);
                storyPhase++;
            }
        }
        else if (storyPhase == 1)
        {
            if (currentPosition < level_1_end.Length - 1)
                dialogueBox.text = level_1_end[++currentPosition];
            else
            {
                GameObject.FindObjectOfType<EnemySpawner>().gameObject.SetActive(false);
                GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in AllEnemies)
                {
                    enemy.GetComponent<EnemyAI>().TakeDamage(1000f);
                }
                currentPosition = 0;
                dialogueBox.text = level_2_end[currentPosition];
                gameManager.ResumeGame();
                father.SetActive(false);
                textBox.SetActive(false);
                storyPhase++;
                ProgressToLevel2();
            }
        }
        else if (storyPhase == 2)
        {
            if (currentPosition < level_2_end.Length - 1)
                dialogueBox.text = level_2_end[++currentPosition];
            else
            {
                GameObject.FindObjectOfType<EnemySpawner>().gameObject.SetActive(false);
                GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in AllEnemies)
                {
                    enemy.GetComponent<EnemyAI>().TakeDamage(1000f);
                }
                currentPosition = 0;
                dialogueBox.text = level_2_end[currentPosition];
                gameManager.ResumeGame();
                father.SetActive(false);
                textBox.SetActive(false);
                storyPhase++;
                ProgressToLevel3();
            }
        }

        else if (storyPhase == 3)
        {
            if (currentPosition < level_2_end.Length - 1)
                dialogueBox.text = level_2_end[++currentPosition];
            else
            {
                gameManager.ResumeGame();
                father.SetActive(false);
                textBox.SetActive(false);
                varian.GetComponent<Varian>().SendMessage("DisableMovement");
                Credits();
            }
        }
    }

    void ContinueStory()
    {
        StartCoroutine("DelayedContinueStory");
    }

    IEnumerator DelayedContinueStory()
    {
        yield return new WaitForSeconds(3f);

        gameManager.StoryPause();
        textBox.SetActive(true);
        father.SetActive(true);
        
    }

    public void ProgressToLevel2()
    {
        varian.gameObject.transform.position = new Vector3(1f, -3f, 0f);
        SceneManager.LoadScene("Level_2");
    }

    public void ProgressToLevel3() 
    {
        varian.gameObject.transform.position = new Vector3(-21.5f, -13f, 0f);
        SceneManager.LoadScene("Level_3");
    }

    public void Credits()
    {
        creditsScreen.SetActive(true);
        StartCoroutine(FadeInCredits());
    }

    private IEnumerator FadeInCredits()
    {
        yield return new WaitForSeconds(2f);

        Image image = creditsScreen.GetComponent<Image>();
        Color color = image.color;
        float alpha = color.a;

        Text text = creditsText.GetComponent<Text>();
        Color textColor = text.color;

        // Gradually increase the alpha value from the current to 1
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            image.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
            text.color = new Color(textColor.r, textColor.g, textColor.b, Mathf.Clamp01(alpha));
            yield return null;
        }
    }
}
