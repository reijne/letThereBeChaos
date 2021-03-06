using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point {
  public Color color;
  public int age;
  public int alive;
}

public struct Spread {
  public Color color;
  public uint pattern;
}

public class EyeComputer : MonoBehaviour {
  [SerializeField] Camera cam;
  [SerializeField] Board board;
  [SerializeField] ComputeShader computeShader;
  [SerializeField] ComputeBuffer pointsBuffer;
  [SerializeField] ComputeBuffer spreadsBuffer;
  [SerializeField] RenderTexture renderTexture;
  private Dictionary<Vector2, uint> positionalMap;
  private Point[] points;
  private Spread[] spreads;
  private int spreadID = 0;
  private int colourID = 0;

  private void Start() {

  }

  public void initialisation() {
    positionalMap = new Dictionary<Vector2, uint>() {
      {new Vector2(1, 1), 128}, // NW
      {new Vector2(0, 1), 64},   // N
      {new Vector2(-1, 1), 32},   // NE

      {new Vector2(1, 0), 16},  // W
      {new Vector2(0, 0), 0},  // 0
      {new Vector2(-1, 0), 8},   // E

      {new Vector2(1, -1), 4},   // SW
      {new Vector2(0, -1), 2},   // S
      {new Vector2(-1, -1), 1},   // SE
    };
    points = new Point[board.width * board.height];
    spreads = new Spread[Pattern.pallete.Count];
    addPoints();
    addSpreads();
    makeRenderTexture();
  }

  public void preparation() {
    int colorSize = sizeof(float) * 4;
    int intSize = sizeof(int);
    int uintSize = sizeof(uint);

    int pointsize = (colorSize + intSize + intSize);
    int spreadsize = (colorSize + uintSize);

    pointsBuffer = new ComputeBuffer(points.Length, pointsize);
    pointsBuffer.SetData(points);
    computeShader.SetBuffer(0, "points", pointsBuffer);

    spreadsBuffer = new ComputeBuffer(spreads.Length, spreadsize);
    spreadsBuffer.SetData(spreads);
    computeShader.SetBuffer(0, "spreads", spreadsBuffer);

    computeShader.SetInt("height", board.height);
    computeShader.SetInt("width", board.width);
    computeShader.SetInt("lifeTime", board.lifeTime);
    computeShader.SetInt("deathTimeMult", board.deathTimeMult);
    computeShader.SetInt("patternCount", spreadID);
    computeShader.SetTexture(0, "board", renderTexture);
    // Debug.Log(points[1].color);
  }

  public void cleanup() {
    if (pointsBuffer != null) pointsBuffer.Dispose();
    if (spreadsBuffer != null) spreadsBuffer.Dispose();
  }

  private void OnApplicationQuit() {
    cleanup();
  }

  public void doCompute() {
    computeShader.Dispatch(0, points.Length / 420, 1, 1);
    pointsBuffer.GetData(points);
    // Debug.Log(points[points.Length - 1].age);
    // Debug.Log(points[2].age);
  }

  private void addPoints() {
    for (int y = 0; y < board.height; y++) {
      for (int x = 0; x < board.width; x++) {
        Point p = new Point();
        p.color = new Color(0, 0, 0, 1);
        p.age = 0;
        p.alive = 1;
        points[x + y * board.width] = p;
      }
    }
    // points[0].color = Pattern.pallete[0];
    // points[points.Length - 1].color = Pattern.pallete[Pattern.pallete.Count - 1];
  }

  private void addSpreads() {
    // foreach (Vector2 pos in Pattern.pattern) {
    spreadID = 0;
    foreach (Color c in Pattern.pallete) {
      Spread s = new Spread();
      s.color = c;
      s.pattern = 0;
      for (int i = 0; i < Pattern.pattern.Count; i++) {
        s.pattern += positionalMap[Pattern.pattern[i]];
      }
      spreads[spreadID] = s;
      spreadID++;
    }
    // }
    // foreach (KeyValuePair<Color, List<Vector2>> kvp in Controls.patterns) {
    //   Spread s = new Spread();
    //   s.color = kvp.Key;

    //   s.pattern = 0;
    //   for (int i = 0; i < kvp.Value.Count; i++) {
    //     s.pattern += positionalMap[kvp.Value[i]];
    //   }
    //   spreads[spreadID] = s;
    //   spreadID++;
    // }
  }

  private void makeRenderTexture() {
    renderTexture = new RenderTexture(board.width, board.height, 24);
    renderTexture.enableRandomWrite = true;
    renderTexture.Create();
  }

  private void makeComputer() {
    computeShader.SetTexture(0, "Result", renderTexture);
    computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
  }

  private void OnRenderImage(RenderTexture src, RenderTexture dest) {
    Graphics.Blit(renderTexture, dest);
  }

  public void doAdditions() {
    manualAddition();
    for (int _ = 0; _ < board.randomPerTick; _++) randomAddition();
    pointsBuffer.SetData(points);
  }

  public (Dictionary<Color, int>, int) countColours() {
    Dictionary<Color, int> colourCounts = new Dictionary<Color, int>();
    foreach (Color c in Pattern.pallete) {
      Debug.Log(String.Format("Adding colour :: {0}", c));
      colourCounts[c] = 0;
    }
    int total = 0;
    if (points != null) {
      foreach (Point p in points) {
        // Debug.Log(String.Format("counting colour :: {0}", p.color));
        if (colourCounts.ContainsKey(p.color)) {
          colourCounts[p.color]++;
          total++;
        }// else colourCounts[p.color] = 1;
      }
    }

    return (colourCounts, total);
  }

  private void manualAddition() {
    if (Input.GetButton("Fire1")) {
      Vector2 mousepos = Input.mousePosition;
      int x = (int)(mousepos.x / Screen.width * board.width);
      int y = (int)(mousepos.y / Screen.height * board.height);
      // Debug.Log(String.Format("mousepos {0}", mousepos));
      // Debug.Log(String.Format("x, y {0}, {1}", x, y));
      // Debug.Log(String.Format("scree {0}, {1}", Screen.width, Screen.height));
      if (Pattern.selected != null) {
        // manualStamp(x, y);
        if (board.eraserMode) manualStamp(x, y, true);
        else manualStamp(x, y);
        // pointsBuffer.SetData(points);

        // pointsBuffer.SetData(points);
        // doCompute();
        // doCompute();
      }
    }
  }

  private void randomAddition() {
    int rx = UnityEngine.Random.Range(0, board.width);
    int ry = UnityEngine.Random.Range(0, board.height);
    colourID = (colourID + 1) % Pattern.pallete.Count;
    // int rid = UnityEngine.Random.Range(0, Pattern.pallete.Count);
    Color rc = Pattern.pallete[colourID];

    if (board.levels[board.lid].hardcore) { // if hardcore dont spawn the desired colour
      colourID = (colourID + 1) % Pattern.pallete.Count;
      rc = Pattern.pallete[colourID];
    }

    if (rc != Color.black) updatePoint(rx, ry, rc);
  }

  private void manualStamp(int x, int y, bool erase = false) {
    for (int dx = -board.eraserSize; dx < board.eraserSize; dx++) {
      for (int dy = -board.eraserSize; dy < board.eraserSize; dy++) {
        Color c = erase ? Color.black : Pattern.selected;
        updatePoint(x + dx, y + dy, c, erase);
      }
    }
  }

  private void updatePoint(int x, int y, Color color, bool erase = false) {
    if (x < 0 || x >= board.width || y < 0 || y >= board.height) return;

    points[x + y * board.width].color = color;
    points[x + y * board.width].age = -2;

    if (erase) points[x + y * board.width].alive = 0;
    else points[x + y * board.width].alive = 1;
  }
}
