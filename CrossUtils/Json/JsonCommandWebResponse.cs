using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CrossUtils.Extensions;

namespace CrossUtils.Json
{
	/// <summary>
	/// Diese Klasse dient dazu, eine WebResponse zu definieren, die dann als Json serialisiert in die RetValue
	/// von einem JsonCommandRetValue gespeichert wird.
	/// Dadurch gibt der JsonCommandWebserver nicht ein serialisierte JsonCommandRetValue dem Client im Content zurück,
	/// sondern direkt den Byte-Stream der Response. Auf diesem Wege können z.B direkt Bilder oder Dateien zurückgegeben
	/// werden, die mit einem JsonCommand angefordert werden können.
	/// Es können hier z.B. durch eine direkte Url Webresourcen übertragen werden, die aber alle vom JsonService zurückgegeben
	/// werden. 
	/// Es gibt noch eine Helper-Methode, wo ein JsonCommand in eine URL umgewandelt werden kann, die dann direkt
	/// an ein HTML-Steuerelement gebunden werden kann. Dazu siehe JsonCommandClient.BuildCommandUrl()
	/// </summary>
	public class JsonCommandWebResponse {
		/// <summary>
		/// gibt den HTML-Konformen Content-Type an. Z.B "image/png" oder "text/plain" oder auch "text/html"
		/// </summary>
		public string ContentType { get; set; }
		/// <summary>
		/// beinhaltet den eigentlichen Content. Dieser Content muss im base64-Format hier abgelegt werden und wird
		/// dann, wenn es zum WebBrowser geschickt wird, wieder in einen Byte-Stream umgewandelt und versendet
		/// </summary>
		public string Content { get; set; }

		public JsonCommandWebResponse() {
			this.ContentType = "";
			this.Content = "";
		}
		public JsonCommandWebResponse(string contentType, string content) {
			this.ContentType = contentType;
			this.Content = content;
		}
		public JsonCommandWebResponse(string contentType, byte[] content) {
			this.ContentType = contentType;
			this.Content = content.ToBase64String();	// Utils.StringUtil.ByteArrayToBase64(content);
		}
		public JsonCommandWebResponse(string filePath) {
			if (File.Exists(filePath)) {
				FileInfo fi = new FileInfo(filePath);
				if (CrossUtils.IO.IOUtil.IsImageFileExtension(fi.Extension))
					this.ContentType = "image/" + fi.Extension.ToLower().Trim(".".ToCharArray());
				else
					this.ContentType = "application/" + fi.Extension.ToLower().Trim(".".ToCharArray());
				this.Content = CrossUtils.IO.IOUtil.FileToBase64(filePath);
			}
		}
	}
}
