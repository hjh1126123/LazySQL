using System;

[Serializable]
public class ExecuteNonModel
{
    private int result;
    private string message;
    private bool success;

    public string Message { get => message; set => message = value; }
    public bool Success { get => success; set => success = value; }
    public int Result { get => result; set => result = value; }
}

