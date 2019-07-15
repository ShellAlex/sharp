using System;
using System.Net;
using NLog;
using NLog.Config;
using System.IO;
using System.Text;
using System.Xml;

namespace gittest
{
    public class HttpListenServer
    {
        public static Logger log;
        public HttpListenServer()
        {
             log = GetLog();
        }

        public static Logger GetLog()
        {
            Logger log = LogManager.GetCurrentClassLogger();
            var conf = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget() { 
            FileName="./logs/${shortdate}", Name = "f"
            };
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug,logFile));
            LogManager.Configuration = conf;
            return log;
        }

        public static void StartListen()
        {
            using (var web = new HttpListener())
            {
                web.Prefixes.Add("http://127.0.0.1:8999/");
                Console.WriteLine("Слушаю....");
                try { 
                web.Start();
                    while (true)
                    {
                        HttpListenerContext context = web.GetContext();
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;
                        Console.WriteLine(
                        request.HttpMethod
                            );

                        string testresp = $"<html>" +
                           $"<body>" +
                            $"<h1>Ответ от недосервера</h1>" +
                            $"</body>" +
                            $"</html>";
                       
                        var  xml = getXmlTestStr().InnerXml;

                        response.AddHeader("Content-type", "text/html;charset=utf8");
                        Cookie cookie = request.Cookies["test"];
                       // if (cookie.Value.Length != 0)
                       if(!String.IsNullOrEmpty(cookie.Value))
                        {
                            Console.WriteLine($"COOKA: {cookie.Value}");
                            //int v = int.Parse(cookie.Value);
                            int  v; 
                            if(!int.TryParse(cookie.Value, out v))
                            {
                                response.Cookies.Add(new Cookie("test", 22.ToString()));
                            }
                            Console.WriteLine($"vPARSA{v}");
                            response.Cookies.Add(new Cookie("test", (v+=25).ToString()));
                        }



                        byte[] respbyte = Encoding.UTF8.GetBytes(xml);
                        response.ContentLength64 = respbyte.Length;

                        Stream outStr = response.OutputStream;
                        outStr.Write(respbyte, 0, respbyte.Length);
                        outStr.Close();
                        //web.Stop();
                    }

                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                }
            }
        }

        public static XmlDocument getXmlTestStr()
        {
             Stream s = new MemoryStream();
           // StringBuilder s = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
           // XmlWriter xml = XmlWriter.Create(s);
            {

                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);

                writer.WriteStartDocument();
                writer.WriteStartElement("People");

                writer.WriteStartElement("Person");
                writer.WriteAttributeString("Name", "Nick");
                writer.WriteValue("value write");
                writer.WriteEndElement();

                writer.WriteStartElement("Person");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("Nick");
                writer.WriteEndAttribute();
                writer.WriteEndElement();

                ///
                writer.WriteStartElement("Person");
                writer.WriteAttributeString("Name", "Tom");
                writer.WriteValue("vvalllue");
                writer.WriteEndElement();

                writer.WriteStartElement("Person");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("Nick");
                writer.WriteEndAttribute();

                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(sb.ToString());
                return xmlDocument;
            }
        }
    }
}
