using UnityEngine;

public class MailKitTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => EmailTester.SendEmail();
}
