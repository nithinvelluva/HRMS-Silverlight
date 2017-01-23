using System;
using System.Web;

namespace PrintToPDF.Web.ClientBin
{
  /// <summary>
  /// Summary description for Png2Pdf
  /// </summary>
  public class Png2Pdf : IHttpHandler
  {
    private int _BufferSize = 32768;
    private int _Margin = 54; // 3/4 inch margin

    public void ProcessRequest(HttpContext context)
    {
      if (context.Request.ContentLength > 0)
      {
        System.IO.Stream incomingStream = context.Request.InputStream;  //Get the incoming stream
        System.Drawing.Image img = System.Drawing.Image.FromStream(incomingStream); //Create and image from the stream

        int pageHeight = img.Height  + (_Margin * 2);
        int pageWidth = img.Width + (_Margin * 2);

        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(img, System.Drawing.Imaging.ImageFormat.Png);
        System.IO.MemoryStream pdfStream = new System.IO.MemoryStream();
        //iTextSharp.text.Font pdfFont = iTextSharp.text.Font.
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(pageWidth, pageHeight));
        //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
        iTextSharp.text.pdf.PdfWriter pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, pdfStream);

        //Insure this is set otherwise when the close method is called on the iTextSharp.text.Document the memory stream
        //we are writting to will be closed
        pdfWriter.CloseStream = false;
        
        //Open PDF and add image to it
        pdfDoc.Open();
        pdfDoc.Add(pdfImage);

        //Close the PDF document to commit the changes
        pdfDoc.Close();

        //Set response headers
        context.Response.Clear();
        context.Response.ContentType = "application/pdf";
        context.Response.AppendHeader("Content-Disposition", "attachment; filename=Export.pdf");
        context.Response.AppendHeader("Content-Length", pdfStream.Length.ToString());
        context.Response.Buffer = false;

        //Insure the stream is at the beginning
        if (pdfStream.Position > 0) { pdfStream.Position = 0; };

        //Stream the exported pdf to the client this is needed since the file size could be large in size
        Byte[] bytesRead = new Byte[_BufferSize];

        long toRead = pdfStream.Length; //Var used to determine how much to read

        while (toRead > 0 && context.Response.IsClientConnected)
        {
          //Insure that _BufferSize do not overrun the end of the stream
          if (_BufferSize > toRead)
          {
            _BufferSize = (int)toRead;
            bytesRead = new Byte[_BufferSize];
          };

          //Write pdf memory stream to bytesRead
          pdfStream.Read(bytesRead, 0, _BufferSize);

          //Read read bytes to outputStream
          context.Response.OutputStream.Write(bytesRead, 0, _BufferSize);

          //Flush response
          context.Response.Flush();

          //Create new byte array to store bytes to read
          bytesRead = new Byte[_BufferSize];

          //Update value used to check for end of stream
          toRead -= _BufferSize;
        }

        //Flush and close the stream
        context.Response.OutputStream.Flush();
        context.Response.End();

        //Close and displose of the memory stream
        if (pdfStream != null)
        {
          pdfStream.Close();
          pdfStream.Dispose();
        }
      }
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }
  }
}