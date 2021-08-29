using System;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {
  public static int SIZE = 3;
  public static int tileCount = 9;
  public static List<Vector2> relPositions = new List<Vector2>();
  public static Color selected;
  public static List<Color> pallete = new List<Color>();
  // [SerializeField] Image patternTile;
  // [SerializeField] Image backdrop;
  // [SerializeField] RectTransform rect;
  public static List<Vector2> pattern = new List<Vector2>();
  // [SerializeField] public Color color;
  private void Awake() {
    // if (relPositions.Count == 0) makeRelativePositions();
    // if (!pallete.Contains(color)) {
    //   pallete.Add(color);
    //   // Debug.Log("Adding to pallete");
    // }
    // Debug.Log(color);
    // patternTile.color = color;
    // createPattern();
    // Controls.patterns[color] = pattern;
    // if (color == Color.black) select();
  }

  private static void makeRelativePositions() {
    relPositions = new List<Vector2>();
    int boundary = (SIZE - 1) / 2;
    for (int y = -boundary; y <= boundary; y++) {
      for (int x = -boundary; x <= boundary; x++) {
        relPositions.Add(new Vector2(x, y));
      }
    }
  }

  public static void createPattern() {
    makeRelativePositions();
    pattern = new List<Vector2>();
    List<Vector2> localRels = new List<Vector2>(relPositions);
    Debug.Log(relPositions.Count);
    Debug.Log(localRels.Count);
    Debug.Log(tileCount);
    // pattern = relPositions;
    // if (selected == Color.black) localTileCount = 9;
    if (tileCount >= relPositions.Count) {
      pattern = relPositions;
      return;
    }

    Debug.Log(String.Format("adding {0} tiles", Mathf.Min(tileCount, localRels.Count)));
    for (int _ = 0; _ < Mathf.Min(tileCount, localRels.Count); _++) {
      int id = UnityEngine.Random.Range(0, localRels.Count);
      // spawnTile(localRels[id]);
      pattern.Add(localRels[id]);
      localRels.RemoveAt(id);
    }
    Debug.Log(pattern == relPositions);
    Debug.Log("lol error pls");
    // foreach (Vector2 pos in localRels) {
    //   spawnTile(pos, true);
    // }
  }

  public static void cleanPallete() {
    pallete = new List<Color>();
  }

  public static void addColor(Color c) {
    if (!pallete.Contains(c)) pallete.Add(c);
  }

  // private void spawnTile(Vector2 pos, bool empty = false) {
  //   float tileSize = rect.sizeDelta.y / SIZE;
  //   Vector3 offset = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
  //   Image tileImage = Instantiate(patternTile, transform.position + offset, Quaternion.identity);
  //   tileImage.transform.SetParent(transform);
  //   tileImage.rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
  //   if (empty) tileImage.color = new Color(patternTile.color.r, patternTile.color.g, patternTile.color.b, 0.3f);
  // }

  // public void deselect() {
  //   // backdrop.color = new Color(1, 1, 1, 0);
  // }

  // public void select() {
  //   if (selected != null) selected.deselect();
  //   // backdrop.color = new Color(1, 1, 1, 0.5f);
  //   selected = this;
  // }
}
