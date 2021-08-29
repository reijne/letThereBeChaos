using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interfaze : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] RectTransform goal;
  [SerializeField] Image colourIndicator;
  [SerializeField] public Text midText;
  [SerializeField] Text percentageText;
  [SerializeField] Text endText;
  [SerializeField] GameObject beforStartControls;
  [SerializeField] GameObject boardGroup;
  [SerializeField] GameObject endScreen;
  [SerializeField] GameObject titleScreen;
  [SerializeField] Button nextButton;
  [SerializeField] Button startButton;
  [SerializeField] RectTransform timer;
  [SerializeField] Image timeleftImage;
  [SerializeField] AudioClip winSound;
  [SerializeField] AudioClip lossSound;
  private bool showing = true;

  // Always set colour with opacity!!! to see the board
  public Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

  public void initInterface() {
    beforStartControls.SetActive(true);
    nextButton.gameObject.SetActive(true);
    boardGroup.SetActive(true);
    endScreen.SetActive(false);
    titleScreen.SetActive(false);
    // reset the timer
    resetTimer();
    // set the desired percentage
    int percent = board.levels[board.lid].desiredPercentage;
    percentageText.text = percent.ToString() + "%";
    // set the desired colour and width according to percent
    colourIndicator.color = new Color(Pattern.selected.r, Pattern.selected.g, Pattern.selected.b, 0.5f);
    float percentWidth = goal.sizeDelta.x * percent / 100;
    colourIndicator.rectTransform.sizeDelta = new Vector2(percentWidth, colourIndicator.rectTransform.sizeDelta.y);
    colourIndicator.rectTransform.localPosition = new Vector3(percentWidth / 2 - goal.sizeDelta.x / 2, 0, 0);
  }

  public void setMidText(string msg) {
    midText.text = msg;
  }

  public void startInterface() {
    boardGroup.SetActive(true);
    endScreen.SetActive(false);
    titleScreen.SetActive(false);
  }

  private void resetTimer() {
    // reset the timerLeftImage to be of width equal to timer.rect
    timeleftImage.rectTransform.sizeDelta = timer.sizeDelta;
    timeleftImage.rectTransform.localPosition = new Vector3(0, 0, 0);
  }

  public void updateTimer() {
    // set the timerleftImage width equal to (board.endTime - board.startingtime) / totalTime
    float remainingTime = (Time.time - board.startingTime) / (board.endTime - board.startingTime);
    Vector2 size = new Vector2(Mathf.Clamp(remainingTime * timer.sizeDelta.x * 1.1f, 0, timer.sizeDelta.x), timer.sizeDelta.y);
    timeleftImage.rectTransform.sizeDelta = size;
    timeleftImage.rectTransform.localPosition = new Vector3(size.x / 2 - timer.sizeDelta.x / 2, 0, 0);
  }

  public void startGameButton() {
    board.startGame();
  }

  public void hideControl() {
    beforStartControls.SetActive(false);
  }

  public void showTitle() {
    boardGroup.SetActive(false);
    endScreen.SetActive(false);
    titleScreen.SetActive(true);
    // loop over all levels and show some graphic
  }

  public void showEnd(int reachedCount, int reachedPercentage, bool win) {
    boardGroup.SetActive(false);
    titleScreen.SetActive(false);
    endScreen.SetActive(true);
    nextButton.gameObject.SetActive(win);

    string end;
    if (win)
      end = string.Format("Chaos could not beat you!!\nYou created {0} coloured cubes\nMaking up for {1}%", reachedCount, reachedPercentage);
    else
      end = string.Format("Chaos beat you..\nYou created {0} coloured cubes\nMaking up for {1}%", reachedCount, reachedPercentage);

    endText.text = end;
  }

  public void nextLevel() {
    board.lid++;
    if (board.lid >= board.levels.Count) showTitle();
    else board.loadLevel(board.lid);
  }

  public void quit() {
    Application.Quit();
  }

  public void showLoss() {
    boardGroup.SetActive(false);
  }
}
