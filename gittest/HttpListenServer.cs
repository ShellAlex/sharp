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

                        XmlText xml = getXmlTestStr();
                        response.AddHeader("Content-type", "text/html;charset=utf8");
                        byte[] respbyte = Encoding.UTF8.GetBytes(testresp);
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

        public static XmlText getXmlTestStr(Stream s)
        {

            XmlWriter xml = XmlWriter.Create(s);
            xml.WriteStartDocument();
            xml.WriteStartElement("Класс");

            xml.WriteStartElement("ЖЫВОТНОЕ");
            xml.WriteAttributeString("АЛЕНЬ","рыжий");
            xml.WriteEndElement();

            xml.WriteStartElement("ЖЫВОТНОЕ");
            xml.WriteStartAttribute("кот", "сереньки");
            xml.WriteValue("сбежавши");
            xml.WriteEndAttribute();
            xml.WriteEndElement();

            xml.WriteStartElement("ЖЫВОТНОЕ");
            xml.WriteAttributeString("сабак", "черны");
            xml.WriteEndElement();

            return xml;

        }
    }
}
