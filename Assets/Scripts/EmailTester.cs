using System.IO;
using MimeKit;
using UnityEngine;

public static class EmailTester
{
    public static void SendEmail()
    {
        // [발신자 설정]
        // Gmail을 사용할 경우:
        // 1. 2단계 인증을 반드시 활성화해야 합니다
        // 2. 앱 비밀번호를 생성해야 합니다 (Google 계정 > 보안 > 앱 비밀번호)
        const string senderName = ""; // 비워두면 이메일 주소만 표시됨
        const string senderEmail = "";
        const string senderPassword = ""; // Gmail의 경우 앱 비밀번호 사용

        // [수신자 설정]
        // 다수의 수신자를 추가하려면 message.To.Add()를 여러 번 호출하면 됩니다
        const string receiverName = ""; // 비워두면 이메일 주소만 표시됨
        const string receiverEmail = "";

        // 이메일 제목
        const string subject = "Help Email Subject";

        // 이메일 본문
        const string body = "Help Email Body";

        // 첨부파일 경로
        const string _attachmentImagePath = "";
        const string _attachmentTextPath = "";

        var message = new MimeMessage();
        // 1. senderName에 string.empty 입력 시, 별도의 이름 기재 없이, senderEmail 주소만 표시됨.
        // 2. senderName에 이름 기재 시, senderEmail 주소와 함께 표시됨.
        message.From.Add(new MailboxAddress(senderName, senderEmail));

        // 1. receiverName에 string.empty 입력 시, 별도의 이름 기재 없이, receiverEmail 주소만 표시됨.
        // 2. receiverName에 이름 기재 시, receiverEmail 주소와 함께 표시됨.
        message.To.Add(new MailboxAddress(receiverName, receiverEmail));

        // 이메일 제목을 기재 합니다.
        message.Subject = subject;

        // 이메일의 본문을 기재합니다.
        // 해당 본문은 파일과 텍스트가 혼합되는 형태의 예제입니다.
        var multipartBody = new Multipart("mixed");
        {
            // 이메일 본문
            var textPart = new TextPart("plain") { Text = body };
            multipartBody.Add(textPart);

            // 첨부파일 - 이미지
            string attachmentPath = _attachmentImagePath;
            var attachmentPart = new MimePart("image/png")
            {
                Content = new MimeContent(File.OpenRead(attachmentPath), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(attachmentPath),
            };
            multipartBody.Add(attachmentPart);

            // 첨부파일 - 텍스트
            string logPath = _attachmentTextPath;
            var logPart = new MimePart("text/plain")
            {
                Content = new MimeContent(File.OpenRead(logPath), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(logPath),
            };
            multipartBody.Add(logPart);
        }
        message.Body = multipartBody;

        // 이메일 전송을 위한 클라이언트를 생성합니다.
        using var client = new MailKit.Net.Smtp.SmtpClient();

        // 클라이언트의 도메인을 설정합니다.
        client.LocalDomain = "localhost";

        // 현재의 예제는 Google 이메일을 사용하고 있습니다.
        // 해당 도메인은 변경 가능합니다.
        // 사용하고자 하는 메일 서비스에 맞춰 설정하십시오.
        // [SMTP 서버 설정]
        // 주요 이메일 서비스 SMTP 설정:
        // - Gmail: smtp.gmail.com, 포트 587
        // - Naver: smtp.naver.com, 포트 587
        // - Daum: smtp.daum.net, 포트 465
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

        // 인증 메커니즘을 제거합니다.
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        // 메일의 전달 주체가 되는 이메일 주소와 비밀번호를 입력합니다.
        // Google 이메일의 경우, 비밀번호는 앱 비밀번호를 사용해야 합니다.
        client.Authenticate(senderEmail, senderPassword);

        // 이메일을 전송합니다.
        client.Send(message);

        // 클라이언트를 종료합니다.
        client.Disconnect(true);

        // 이메일 전송이 완료되었음을 알립니다.
        Debug.Log("Sent email");
    }
}
