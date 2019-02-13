using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace ASPUnhandledException
{
    // '' <summary>
    // '' a simple class for trivial SMTP mail support, with no COM dependencies.
    // '' </summary>
    // '' <remarks>
    // ''   - plain text or HTML body
    // ''   - one optional file attachment
    // ''   - basic retry mechanism
    // '' 
    // ''  Jeff Atwood
    // ''  http://www.codinghorror.com/
    // '' </remarks>
    public class SimpleMail
    {
        // '' <summary>
        // '' A mail message to be sent. The only required properties are To, and Body.
        // '' </summary>
        public class SmtpMailMessage
        {
            // '' <summary>
            // '' Address this email came from. Optional.
            // '' If not provided, an email address will be automatically generated based on the machine name. 
            // '' </summary>
            public string From;

            // '' <summary>
            // '' Address(es) to send email to. Semicolon delimited. Required.
            // '' </summary>
            public string To;

            // '' <summary>
            // '' Subject text for the email. Optional, but recommended.
            // '' </summary>
            public string Subject;

            // '' <summary>
            // '' Plain text body. Required.
            // '' </summary>
            public string Body;

            // '' <summary>
            // '' HTML text body. Optional.
            // '' </summary>
            public string BodyHTML;

            // '' <summary>
            // '' Fully qualified path of the file you want to attach to the email. Optional.
            // '' </summary>
            public string AttachmentPath;

            // '' <summary>
            // '' String you wish to attach to the email. Intended for large strings. Optional.
            // '' </summary>
            public string AttachmentText;

            // '' <summary>
            // '' Name of the attachment as shown in the email. Optional.
            // '' </summary>
            public string AttachmentFilename;
        }

        // '' <summary>
        // '' SMTP client used to submit SMTPMailMessage(s)
        // '' </summary>
        public class SmtpClient
        {
            private const int _intBufferSize = 1024;

            private const int _intResponseTimeExpected = 10;

            private const int _intResponseTimeMax = 750;

            private const string _strAddressSeperator = ";";

            private const int _intMaxRetries = 3;

            private const bool _blnPlainTextOnly = false;

            private string _strDefaultDomain = Config.GetString("SmtpDefaultDomain", string.Empty);

            private string _strServer = Config.GetString("SmtpServer", string.Empty);

            private int _intPort = Config.GetInteger("SmtpPort", 25);

            private string _strUserName = Config.GetString("SmtpAuthUser", string.Empty);

            private string _strUserPassword = Config.GetString("SmtpAuthPassword", string.Empty);

            private bool _blnDebugMode;

            private int _intRetries = 1;

            private string _strLastResponse;

            public SmtpClient()
            {
                _blnDebugMode = Debugger.IsAttached;
            }

            // '' <summary>
            // '' Authenticating username, if your mail server requires outgoing authentication.
            // '' Leave blank otherwise.
            // '' </summary>
            public string AuthUser
            {
                get { return _strUserName; }
                set { _strUserName = value; }
            }

            public string AuthPassword
            {
                get { return _strUserPassword; }
                set { _strUserPassword = value; }
            }

            public int Port
            {
                get { return _intPort; }
                set { _intPort = value; }
            }

            public string Server
            {
                get { return _strServer; }
                set { _strServer = value; }
            }

            public string DefaultDomain
            {
                get { return _strDefaultDomain; }
                set { _strDefaultDomain = value; }
            }

            private bool IsWebHosted()
            {
                return (HttpContext.Current != null);
            }

            // '' <summary>
            // '' send data over the current network connection
            // '' </summary>
            private void SendData(TcpClient tcp,
                string strData)
            {
                var ns = tcp.GetStream();
                var en = new UTF8Encoding();

                var b = en.GetBytes(strData);
                ns.Write(b, 0, b.Length);
            }

            // '' <summary>
            // '' get data from the current network connection
            // '' </summary>
            private string GetData(TcpClient tcp)
            {
                var ns = tcp.GetStream();

                if (ns.DataAvailable)
                {
                    int intStreamSize;

                    var b = new byte[_intBufferSize];

                    intStreamSize = ns.Read(b, 0, b.Length);
                    var en = new UTF8Encoding();

                    return en.GetString(b);
                }

                return string.Empty;
            }

            // '' <summary>
            // '' issue a required SMTP command
            // '' </summary>
            private void Command(TcpClient tcp,
                string strCommand,
                string strExpectedResponse = "250")
            {
                if (!CommandInternal(tcp, strCommand, strExpectedResponse))
                {
                    tcp.Close();
                    // Warning!!! Optional parameters not supported
                    throw new Exception(
                        ("SMTP server at "
                         + (_strServer + (":"
                                          + ((_intPort + " was provided command \'")
                                             + (strCommand + ("\', but did not return the expected response \'"
                                                              + (strExpectedResponse + ("\':"
                                                                                        +
                                                                                        (Environment.NewLine +
                                                                                         _strLastResponse))))))))));
                }
            }

            // '' <summary>
            // '' issue a SMTP command
            // '' </summary>
            private bool CommandInternal(TcpClient tcp,
                string strCommand,
                string strExpectedResponse = "250")
            {
                // Warning!!! Optional parameters not supported
                // -- send the command over the socket with a trailing cr/lf
                if ((strCommand.Length > 0))
                {
                    SendData(tcp, (strCommand + Environment.NewLine));
                }

                // -- wait until we get a response, or time out
                _strLastResponse = string.Empty;
                var intResponseTime = 0;

                while ((string.IsNullOrWhiteSpace(_strLastResponse) && (intResponseTime <= _intResponseTimeMax)))
                {
                    intResponseTime = (intResponseTime + _intResponseTimeExpected);
                    _strLastResponse = GetData(tcp);

                    //Thread.CurrentThread.Sleep(_intResponseTimeExpected);
                    Thread.Sleep(_intResponseTimeExpected);
                }

                // -- this is helpful for debugging SMTP problems
                if (_blnDebugMode)
                {
                    Debug.WriteLine(("SMTP >> " + (strCommand + (" (after " + (intResponseTime + "ms)")))));
                    Debug.WriteLine(("SMTP << " + _strLastResponse));
                }

                // -- if we have a response, check the first 10 characters for the expected response code
                if (string.IsNullOrWhiteSpace(_strLastResponse))
                {
                    if (_blnDebugMode)
                    {
                        Debug.WriteLine(("** EXPECTED RESPONSE " + (strExpectedResponse + " NOT RETURNED **")));
                    }

                    return false;
                }

                return (_strLastResponse.IndexOf(strExpectedResponse, 0, 10) != -1);
            }

            // '' <summary>
            // '' send mail with integrated retry mechanism
            // '' </summary>
            public bool SendMail(SmtpMailMessage mail)
            {
                var intRetryInterval = 333;

                try
                {
                    SendMailInternal(mail);

                }
                catch (Exception ex)
                {
                    _intRetries++;

                    if (_blnDebugMode)
                    {
                        Debug.WriteLine("--> SendMail Exception Caught");
                        Debug.WriteLine(ex.Message);
                    }

                    if ((_intRetries <= _intMaxRetries))
                    {
                        //Thread.CurrentThread.Sleep(intRetryInterval);
                        Thread.Sleep(intRetryInterval);
                        SendMail(mail);

                    }
                    else
                    {
                        throw;
                    }
                }

                if (_blnDebugMode)
                {
                    Debug.WriteLine(("sent after " + _intRetries));
                }

                _intRetries = 1;
                return true;
            }

            // '' <summary>
            // '' send an email via trivial SMTP implementation
            // '' </summary>
            private void SendMailInternal(SmtpMailMessage mail)
            {
                IPHostEntry iphost;
                var tcp = new TcpClient();

                // -- resolve server text name to an IP address
                try
                {
                    iphost = Dns.GetHostByName(_strServer);

                }
                catch (Exception e)
                {
                    throw new Exception(("Unable to resolve server name " + _strServer), e);
                }

                // -- attempt to connect to the server by IP address and port number
                try
                {
                    tcp.Connect(iphost.AddressList[0], _intPort);

                }
                catch (Exception e)
                {
                    throw new Exception(("Unable to connect to SMTP server at " + (_strServer + (":" + _intPort))), e);
                }

                // -- make sure we get the SMTP welcome message
                Command(tcp, string.Empty, "220");
                Command(tcp, ("HELO " + Environment.MachineName));

                // --
                // -- authenticate if we have username and password
                // -- http://www.ietf.org/rfc/rfc2554.txt
                // --
                if (((_strUserName + _strUserPassword).Length > 0))
                {
                    Command(tcp, "auth login", "334 VXNlcm5hbWU6");

                    // VXNlcm5hbWU6=base64'Username:'
                    Command(tcp, ToBase64(_strUserName), "334 UGFzc3dvcmQ6");

                    // UGFzc3dvcmQ6=base64'Password:'
                    Command(tcp, ToBase64(_strUserPassword), "235");
                }

                if (string.IsNullOrWhiteSpace(mail.From))
                {
                    if (IsWebHosted())
                    {
                        mail.From = (HttpContext.Current.Request.ServerVariables["server_name"] +
                                     ("@" + _strDefaultDomain));

                    }
                    else
                    {
                        mail.From = (
                            AppDomain.CurrentDomain.FriendlyName.ToLower() +
                            ("." + (Environment.MachineName.ToLower() +
                                    ("@" + _strDefaultDomain))));
                    }
                }

                Command(tcp, ("MAIL FROM: <" + (mail.From + ">")));

                // -- send email to more than one recipient
                var strRecipients = mail.To.Split(_strAddressSeperator.ToCharArray());

                foreach (var strRecipient in strRecipients)
                {
                    Command(tcp, ("RCPT TO: <" + (strRecipient + ">")));
                }

                Command(tcp, "DATA", "354");
                var sb = new StringBuilder();

                // With...
                // -- write common email headers
                sb.Append(("To: " + (mail.To + Environment.NewLine)));
                sb.Append(("From: " + (mail.From + Environment.NewLine)));
                sb.Append(("Subject: " + (mail.Subject + Environment.NewLine)));

                if (_blnPlainTextOnly)
                {
                    // -- write plain text body
                    sb.Append(Environment.NewLine + (mail.Body + Environment.NewLine));
                }
                else
                {
                    // -- typical case; mixed content will be displayed side-by-side
                    var strContentType = "multipart/mixed";

                    if ((!string.IsNullOrWhiteSpace(mail.Body)) && (!string.IsNullOrWhiteSpace(mail.BodyHTML)))
                    {
                        strContentType = "multipart/alternative";
                    }

                    sb.Append(("MIME-Version: 1.0" + Environment.NewLine));
                    sb.Append(("Content-Type: " +
                               (strContentType + ("; boundary=\"NextMimePart\"" + Environment.NewLine))));
                    sb.Append(("Content-Transfer-Encoding: 7bit" + Environment.NewLine));
                    //  -- default content (for non-MIME compliant email clients, should be extremely rare)
                    sb.Append(("This message is in MIME format. Since your mail reader does not understand " +
                               Environment.NewLine));
                    sb.Append(("this format, some or all of this message may not be legible." + Environment.NewLine));

                    // -- handle text body (if any)
                    if (!string.IsNullOrWhiteSpace(mail.Body))
                    {
                        sb.Append((Environment.NewLine + ("--NextMimePart" + Environment.NewLine)));
                        sb.Append(("Content-Type: text/plain;" + Environment.NewLine));
                        sb.Append((Environment.NewLine + (mail.Body + Environment.NewLine)));
                    }

                    //  -- handle HTML body (if any)
                    if (!string.IsNullOrWhiteSpace(mail.BodyHTML))
                    {
                        sb.Append((Environment.NewLine + ("--NextMimePart" + Environment.NewLine)));
                        sb.Append(("Content-Type: text/html; charset=iso-8859-1" + Environment.NewLine));
                        sb.Append((Environment.NewLine + (mail.BodyHTML + Environment.NewLine)));
                    }

                    // -- handle attachment (if any)
                    if (!string.IsNullOrWhiteSpace(mail.AttachmentPath))
                    {
                        sb.Append(FileToMimeString(mail.AttachmentPath, mail.AttachmentFilename));
                    }

                    if (!string.IsNullOrWhiteSpace(mail.AttachmentText))
                    {
                        sb.Append(ToMimeString(mail.AttachmentText, mail.AttachmentFilename));
                    }
                }

                // -- <crlf>.<crlf> marks end of message content
                sb.Append(Environment.NewLine + ("." + Environment.NewLine));

                Command(tcp, sb.ToString());
                Command(tcp, "QUIT", string.Empty);
                tcp.Close();
            }

            // '' <summary>
            // '' returns MIME header section string
            // '' </summary>
            private string MimeHeaderString(string strFilename)
            {
                var sb = new StringBuilder();

                if (string.IsNullOrEmpty(strFilename))
                {
                    strFilename = "attachment.txt";
                }

                // With...
                sb.Append(Environment.NewLine + "--NextMimePart" + Environment.NewLine);
                sb.Append("Content-Type: application/octet-stream; name=\"" + strFilename + "\"" + Environment.NewLine);
                sb.Append("Content-Transfer-Encoding: base64" + Environment.NewLine);
                sb.Append("Content-Disposition: attachment; filename=\"" + strFilename + "\"" + Environment.NewLine);
                sb.Append(Environment.NewLine);

                return sb.ToString();
            }

            // '' <summary>
            // '' turn string into a MIME attachment string
            // '' </summary>
            private string ToMimeString(string strAny,
                string strFilename = "Attachment.txt")
            {
                var sb = new StringBuilder();
                const int intLineWidth = 75;

                strAny = Convert.ToBase64String(Encoding.Default.GetBytes(strAny));

                sb.Append(MimeHeaderString(strFilename));

                int i;
                var c = 0;

                for (i = 0; (i <= (strAny.Length - 1)); i++)
                {
                    c++;
                    sb.Append(strAny.Substring(i, 1));

                    if ((c == (intLineWidth - 1)))
                    {
                        c = 0;
                        sb.Append(Environment.NewLine);
                    }
                }

                return sb.ToString();
            }

            // '' <summary>
            // '' turn a file into a MIME attachment string
            // '' </summary>
            private string FileToMimeString(string strFilepath,
                string strFileName = "")
            {
                // Warning!!! Optional parameters not supported
                var sb = new StringBuilder();

                // -- note that chunk size is equal to maximum line width
                const int intChunkSize = 75;
                var bytRead = new byte[intChunkSize];

                if (string.IsNullOrEmpty(strFileName))
                {
                    // -- get just the filename out of the path
                    strFileName = Path.GetFileName(strFilepath);
                }

                sb.Append(MimeHeaderString(strFileName));

                var fs = new FileStream(strFilepath, FileMode.Open, FileAccess.Read);
                var intRead = fs.Read(bytRead, 0, intChunkSize);

                while ((intRead > 0))
                {
                    sb.Append(Convert.ToBase64String(bytRead, 0, intRead));
                    sb.Append(Environment.NewLine);
                    intRead = fs.Read(bytRead, 0, intChunkSize);
                }

                fs.Close();
                return sb.ToString();
            }

            // '' <summary>
            // '' Encodes a string as Base64
            // '' </summary>
            private string ToBase64(string data)
            {
                var Encoder = new UTF8Encoding();

                return Convert.ToBase64String(Encoder.GetBytes(data));
            }
        }
    }
}
