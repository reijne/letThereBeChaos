﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pattern : MonoBehaviour {
  public static int SIZE = 3;
  public static int tileCount = 7;
  public static List<Vector2> relPositions = new List<Vector2>();
  public static Pattern selected;
  public static List<Color> pallete = new List<Color>();
  [SerializeField] Image patternTile;
  [SerializeField] Image backdrop;
  [SerializeField] RectTransform rect;
  [SerializeField] public Color color;
  public List<Vector2> pattern = new List<Vector2>();
  private void Start() {
    if (relPositions.Count == 0) makeRelativePositions();
    if (!pallete.Contains(color)) pallete.Add(color);
    patternTile.color = color;
    createPattern();
    Controls.patterns[color] = pattern;
  }

  private void makeRelativePositions() {
    int boundary = (SIZE - 1) / 2;
    for (int y = -boundary; y <= boundary; y++) {
      for (int x = -boundary; x <= boundary; x++) {
        relPositions.Add(new Vector2(x, y));
      }
    }
  }

  private void createPattern() {
    List<Vector2> localRels = new List<Vector2>(relPositions);
    for (int _ = 0; _ < tileCount; _++) {
      int id = Random.Range(0, localRels.Count);
      spawnTile(localRels[id]);
      pattern.Add(localRels[id]);
      localRels.RemoveAt(id);
    }
  }

  private void spawnTile(Vector2 pos) {
    float tileSize = rect.sizeDelta.y / SIZE;
    Debug.Log(rect.sizeDelta.y);
    Debug.Log(tileSize);
    Vector3 offset = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
    Image tileImage = Instantiate(patternTile, transform.position + offset, Quaternion.identity);
    tileImage.transform.SetParent(transform);
    tileImage.rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
  }

  public void deselect() {
    backdrop.color = new Color(1, 1, 1, 0);
  }

  public void select() {
    if (selected != null) selected.deselect();
    backdrop.color = new Color(1, 1, 1, 0.5f);
    selected = this;
  }
}
