using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interfaze : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] Transform indicatorSpawnpoint;
  [SerializeField] Image totalImage;
  [SerializeField] Text totalText;
  [SerializeField] ComputeShader shader;
  [SerializeField] RenderTexture renderTexture;
  public Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

  private void Start() {
    makeRenderTexture();
  }

  private void makeRenderTexture() {
    renderTexture = new RenderTexture(board.width, board.height, 24);
    renderTexture.enableRandomWrite = true;
    renderTexture.Create();
  }
  public void updateColorIndicators(int total) {
    // get width 
  }

  public void addColorCount(Color color) {
    if (colorCounts.ContainsKey(color)) colorCounts[color]++;
    else colorCounts[color] = 1;
  }

  private void Update() {
    // printCounts();
  }
  public void printCounts() {
    foreach (KeyValuePair<Color, int> kvp in colorCounts) {
      Debug.Log(String.Format("{0} : {1}", kvp.Key, kvp.Value));
    }
  }

}
