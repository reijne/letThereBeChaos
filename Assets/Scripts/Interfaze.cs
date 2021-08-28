using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interfaze : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] Transform indicatorSpawnpoint;
  [SerializeField] Image totalImage;
  [SerializeField] Text totalText;
  [SerializeField] GameObject controlBar;
  [SerializeField] GameObject panel;
  [SerializeField] public Button startStop;
  private bool showing = true;

  public Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

  public void updateColorIndicators(int total) {
    // get width 
  }

  public void addColorCount(Color color) {
    if (colorCounts.ContainsKey(color)) colorCounts[color]++;
    else colorCounts[color] = 1;
  }

  private void Update() {
    // printCounts();
    if (Input.GetKeyDown(KeyCode.Escape)) hide();
    if (Input.GetKeyDown(KeyCode.Space)) board.startGame();
  }
  public void printCounts() {
    foreach (KeyValuePair<Color, int> kvp in colorCounts) {
      Debug.Log(String.Format("{0} : {1}", kvp.Key, kvp.Value));
    }
  }

  private void hide() {
    showing = !showing;
    controlBar.SetActive(showing);
    panel.SetActive(showing);
  }

  public void toggleStartStopButton(bool started) {
    Text buttonText = startStop.GetComponentInChildren<Text>();
    if (started) {
      buttonText.text = "stop";
    } else {
      buttonText.text = "start";
    }
  }

}
