using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point {
  public Color color;
  public int age;
}

public struct Spread {
  public Color color;
  public uint pattern;
}

public class EyeComputer : MonoBehaviour {
  [SerializeField] Board board;
  [SerializeField] ComputeShader computeShader;
  [SerializeField] ComputeBuffer pointsBuffer;
  [SerializeField] ComputeBuffer spreadsBuffer;
  [SerializeField] RenderTexture renderTexture;
  private Dictionary<Vector2, uint> positionalMap;
  private Point[] points;
  private Spread[] spreads;
  private int spreadID = 0;

  private void Start() {
    // NW (-1, 1) :: 128
    // N (-1, -1) :: 64
    positionalMap = new Dictionary<Vector2, uint>() {
      {new Vector2(-1, 1), 128}, // NW
      {new Vector2(0, 1), 64},   // N
      {new Vector2(1, 1), 32},   // NE

      {new Vector2(-1, 0), 16},  // W
      {new Vector2(0, 0), 0},  // 0
      {new Vector2(1, 0), 8},   // E

      {new Vector2(-1, -1), 4},   // SW
      {new Vector2(0, -1), 2},   // S
      {new Vector2(1, -1), 1},   // SE
    };
    points = new Point[board.width * board.height];
    spreads = new Spread[Controls.patterns.Count];
    addPoints();
    addSpreads();
    makeRenderTexture();
    preparation();
    // makeComputer();
  }

  public void preparation() {
    int colorSize = sizeof(float) * 4;
    int intSize = sizeof(int);
    int uintSize = sizeof(uint);

    int pointsize = (colorSize + intSize);
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
    computeShader.SetInt("patternCount", Controls.patterns.Count);
    computeShader.SetTexture(0, "board", renderTexture);
    Debug.Log(points[1].color);
  }

  private void OnApplicationQuit() {
    pointsBuffer.Dispose();
    spreadsBuffer.Dispose();
  }

  public void doCompute() {
    computeShader.Dispatch(0, points.Length / 100, 1, 1);
    pointsBuffer.GetData(points);
  }

  private void addPoints() {
    for (int y = 0; y < board.height; y++) {
      for (int x = 0; x < board.width; x++) {
        Point p = new Point();
        p.color = new Color(0, 0, 0, 1);
        p.age = 0;
        points[x + y * board.width] = p;
      }
    }
    // points[0].color = Pattern.pallete[0];
    // points[points.Length - 1].color = Pattern.pallete[Pattern.pallete.Count - 1];
  }

  private void addSpreads() {
    foreach (KeyValuePair<Color, List<Vector2>> kvp in Controls.patterns) {
      Spread s = new Spread();
      s.color = kvp.Key;

      s.pattern = 0;
      for (int i = 0; i < kvp.Value.Count; i++) {
        s.pattern += positionalMap[kvp.Value[i]];
      }
      spreads[spreadID] = s;
      spreadID++;
    }
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

  private void Update() {
    if (Input.GetButtonDown("Fire1")) {
      Vector2 mousepos = Input.mousePosition;
      int x = (int)(mousepos.x / Screen.width * board.width);
      int y = (int)(mousepos.y / Screen.height * board.height);
      // Debug.Log(String.Format("mousepos {0}", mousepos));
      // Debug.Log(String.Format("x, y {0}, {1}", x, y));
      // Debug.Log(String.Format("scree {0}, {1}", Screen.width, Screen.height));
      if (Pattern.selected != null) {
        points[x + y * board.width].color = Pattern.selected.color;
        points[x + y * board.width].age = 0;
        pointsBuffer.SetData(points);
        // doCompute();
      }
    }
  }
}
