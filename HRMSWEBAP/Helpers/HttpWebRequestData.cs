using System;
using System.Net;

namespace HRMSWEBAP
{
    public class HttpWebRequestData
  {
    public HttpWebRequest Request{get; set;}
    public byte[] Data{get; set;}

    public HttpWebRequestData(HttpWebRequest Request, byte[] Data)
    {
      this.Request = Request;
      this.Data = Data;
    }

  }
}
