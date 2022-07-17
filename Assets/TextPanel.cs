using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPanel : MonoBehaviour
{
  public static TextPanel instance;

  [SerializeField]
  TMP_Text text;

  bool done = true;
  string fullText = "";

  public delegate void OnTextFinished();
  public OnTextFinished onTextFinished;
  public OnTextFinished onFullTextShowing;

  Coroutine textCoroutine;
  float timer = 0;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
      gameObject.SetActive(false);
    }
    else
      Destroy(gameObject);

  }

  public void ShowText(string text)
  {
    this.text.text = "";
    gameObject.SetActive(true);
    textCoroutine = StartCoroutine(ShowTextCoroutine(text));
    done = false;
    fullText = text;
    timer = 0;
  }

  public void Hide()
  {
    if (textCoroutine != null)
    {
      StopCoroutine(textCoroutine);
    }
    text.text = "";
    done = true;
    gameObject.SetActive(false);
  }

  void Update()
  {
    if (done)
    {
      timer += Time.deltaTime;
      if ((Input.GetKeyDown(KeyCode.Space) || timer >= 10))
      {
        onTextFinished?.Invoke();
      }
    }
    else if (Input.GetKeyDown(KeyCode.Space))
    {
      Debug.Log("Keycode down space");
      StopCoroutine(textCoroutine);
      text.text = fullText;
      done = true;
      onFullTextShowing?.Invoke();
    }
  }

  IEnumerator ShowTextCoroutine(string text)
  {
    foreach (char c in text)
    {
      this.text.text += c;
      yield return new WaitForSeconds(0.05f);
    }
    onFullTextShowing?.Invoke();
    done = true;
  }
}
