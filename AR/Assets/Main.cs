using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZXing;
using ZXing.QrCode;

public class Main : MonoBehaviour
{
    private WebCamTexture camTexture;
    private Rect screenRect;
    public GameObject queenPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;

    private Transform queen;
    private Transform rook;
    private Transform knight;
    private float window = 0.3f;
    public float[,] Base;
    public float[,] End;

    void Start()
    {
        Camera();
        ChessStart();
    }

    void Update()
    {
        ChessMoves();
    }

    #region QR

    void Camera()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }

    void OnGUI()
    {
        // drawing the camera on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
              camTexture.width, camTexture.height);
            if (result != null)
            {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }
    private static Color32[] Encode(string textForEncoding, int width, int height) // reading and writing QR
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
    public Texture2D generateQR(string text) // generate a 2D texture and display in in the GUI
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    #endregion QR

    #region Location

    IEnumerator CheckLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        /*
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        */
    }

    #endregion Location

    #region Chess

    void ChessStart()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        queen = queenPrefab.transform;
        rook = rookPrefab.transform;
        knight = knightPrefab.transform;
        queen.transform.position = new Vector3(0f, 0f, 0f);
        rook.transform.position = new Vector3(0.0f, 0.0f, 0f);
        knight.transform.position = new Vector3(0.0f, 0.0f, 0f);
    }

    void ChessMoves()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(horizontalInput * window, verticalInput * window, 0);
        CheckIfDone();
    }
    void CheckIfDone()
    {
        for (int i = 0; i < Base.Length; i++)
            if (End[i, i] == Base[i, i])
                GameOver();
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    #endregion Chess
}