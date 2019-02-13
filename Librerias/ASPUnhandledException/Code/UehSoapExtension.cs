using System.IO;
using System.Web.Services.Protocols;

namespace ASPUnhandledException
{
    // '' <summary>
    // '' Implements ASP unhandled exception manager as a SoapExtension
    // '' </summary>
    // '' <remarks>
    // '' to use:
    // '' 
    // ''    1) place ASPUnhandledException.dll in the \bin folder
    // ''    2) add this section to your Web.config under the <webServices> element:
    // ''            <!-- Adds our error handler to the SOAP pipeline. -->
    // ''            <soapExtensionTypes>
    // ''                <add type="ASPUnhandledException.UehSoapExtension, ASPUnhandledException"
    // ''                     priority="1" group="0" />
    // ''            </soapExtensionTypes>
    // ''
    // ''  Jeff Atwood
    // ''  http://www.codinghorror.com/
    // '' </remarks>
    public class UehSoapExtension : SoapExtension
    {
        private Stream _OldStream;

        private Stream _NewStream;

        public override object GetInitializer(System.Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo,
            SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
        }

        public override Stream ChainStream(Stream stream)
        {
            _OldStream = stream;
            _NewStream = new MemoryStream();
            return _NewStream;
        }

        private void Copy(Stream fromStream, Stream toStream)
        {
            var sr = new StreamReader(fromStream);
            var sw = new StreamWriter(toStream);
            sw.Write(sr.ReadToEnd());
            sw.Flush();
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    Copy(_OldStream, _NewStream);
                    _NewStream.Position = 0;
                    break;

                case SoapMessageStage.AfterSerialize:
                    if (message.Exception != null)
                    {
                        var ueh = new Handler();

                        // -- handle our exception, and get the SOAP <detail> string
                        var strDetailNode = ueh.HandleWebServiceException(message);

                        // -- read the entire SOAP message stream into a string
                        _NewStream.Position = 0;
                        TextReader tr = new StreamReader(_NewStream);

                        // -- insert our exception details into the string
                        var s = tr.ReadToEnd();
                        s = s.Replace("<detail />", strDetailNode);

                        // -- overwrite the stream with our modified string
                        _NewStream = new MemoryStream();
                        TextWriter tw = new StreamWriter(_NewStream);

                        tw.Write(s);
                        tw.Flush();
                    }

                    _NewStream.Position = 0;
                    Copy(_NewStream, _OldStream);
                    break;
            }
        }
    }
}
