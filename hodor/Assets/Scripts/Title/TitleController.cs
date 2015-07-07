using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TitleController : SimpleViewController
{
    private const float splashTimeout = 1.0f;
    public TextAsset StoryText;
    public Button SkipButton;
    public Text StoryLabel;

    private List<StoryTextEntry> story;

    internal struct StoryTextEntry
    {
        public float Duration;
        public string Text;

        public StoryTextEntry(float duration, string text)
        {
            Duration = duration;
            Text = text;
        }

        public static List<StoryTextEntry> ParseText(TextAsset textAsset)
        {
            List<StoryTextEntry> list = new List<StoryTextEntry>();
            string[] lmnts;

            foreach (string line in textAsset.text.Replace("\r\n","\n").Split('\n'))
            {
                lmnts = line.Split('\t');
                if (lmnts.Length != 2) continue;

                StoryTextEntry entry;
                entry.Duration = float.Parse(lmnts[0]);
                entry.Text = lmnts[1];

                list.Add(entry);
            }

            return list;
        }
    }

    void OnEnable()
    {
        SkipButton.onClick.AddListener(() => Application.LoadLevel("Menu"));

        story = StoryTextEntry.ParseText(StoryText);

        StartCoroutine(ShowIntro());
    }

    IEnumerator ShowIntro()
    {
        foreach (StoryTextEntry entry in story)
        {
            StoryLabel.text = entry.Text;
            yield return new WaitForSeconds(entry.Duration);
        }

        Application.LoadLevel("Menu");
    }
}
