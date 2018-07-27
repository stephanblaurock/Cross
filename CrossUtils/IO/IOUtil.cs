using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrossUtils.IO
{
	public class IOUtil {
		
		public static string StringFromFile(string filename, System.Text.Encoding encoding) {
			FileInfo fi = new FileInfo(filename);
			if (fi.Exists) {
				string content = "";
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
					using (StreamReader sr = new StreamReader(fs)) {        // filename, encoding);
						content = sr.ReadToEnd();
						//while (!sr.EndOfStream) {
						//	sr.ReadToEnd
						//}
						//string content = sr.ReadToEnd();
						//sr.Close();
					}
				}
				return content;
			}
			return "";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		/// <param name="encoding"></param>
		/// <param name="ifFileExistSaveItInSeperateFile">Wenn true dann wird der Dateinamen mit einer Nummer erweitert z.b beispiel(2).txt</param>
		public static void StringToFile(string filename, string content, System.Text.Encoding encoding, bool ifFileExistSaveItInSeperateFile = false) {
			StreamWriter sw = new StreamWriter(ifFileExistSaveItInSeperateFile ? GetNotExistingFileName(filename) : filename, false, encoding);
			sw.Write(content);
			sw.Close();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="content"></param>
		/// <param name="ifExistSaveItInSeperateFile">Wenn true dann wird der Dateinamen mit einer Nummer erweitert z.b beispiel(2).txt</param>
		public static void StringToFile(string filename, string content, bool ifFileExistSaveItInSeperateFile = false) {
			FileInfo fi = new FileInfo(ifFileExistSaveItInSeperateFile ? GetNotExistingFileName(filename) : filename);
			StreamWriter sw = fi.CreateText();
			sw.Write(content);
			sw.Close();
		}
		private static string GetNotExistingFileName(string filename, int counter = 0) {
			if (File.Exists(filename)) {
				FileInfo fi = new FileInfo(filename);
				int pos = filename.LastIndexOf(fi.Extension);
				string newName = filename.Insert(pos, $"({counter.ToString()})");
				//Debug.WriteLine($"neuer Name = {newName} alter Name = {filename}");
				return GetNotExistingFileName(filename, counter++);
			} else
				return filename;
		}
		public static void StreamToFile(string filename, Stream content) {
			content.Seek(0, System.IO.SeekOrigin.Begin);
			FileStream fStream = new FileStream(filename, FileMode.Create);
			byte[] bytesToWrite = new byte[content.Length];
			content.Read(bytesToWrite, 0, Convert.ToInt32(content.Length));
			fStream.Write(bytesToWrite, 0, bytesToWrite.Length);
			fStream.Flush();
			fStream.Close();
		}
		public static Stream StreamFromFile(string filename) {
			FileStream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			MemoryStream destStream = new MemoryStream();
			byte[] buffer = new byte[32768];
			int read = 0;
			while ((read = fStream.Read(buffer, 0, buffer.Length)) > 0) {
				destStream.Write(buffer, 0, read);
			}
			destStream.Flush();
			fStream.Close();
			//decompressedText = System.Text.Encoding.Default.GetString( destinationStream.ToArray() );
			return destStream;
		}
		public static void CopyDirectory(string Src, string Dst) {
			String[] Files;
			if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
				Dst += Path.DirectorySeparatorChar;
			if (!Directory.Exists(Dst))
				Directory.CreateDirectory(Dst);
			Files = Directory.GetFileSystemEntries(Src);
			foreach (string Element in Files) {
				// Sub directories
				if (Directory.Exists(Element))
					CopyDirectory(Element, Dst + Path.GetFileName(Element));
				// Files in directory
				else
					File.Copy(Element, Dst + Path.GetFileName(Element), true);
			}
		}
		//public static Stream String2Stream(string s) {
		//	byte[] buf = System.Text.Encoding.GetEncoding(1252).GetBytes(s);
		//	MemoryStream ms = new MemoryStream(buf);
		//	return (Stream)ms;
		//}
		public static string Stream2String(Stream stream) {
			StreamReader sr = new StreamReader(stream);
			string retval = sr.ReadToEnd();
			sr.Close();
			sr.Dispose();
			return retval;
		}

		/// <summary>
		/// Diese Funktion entfernt alle Zeichen aus dem übergebenen String
		/// die in Dateinamen nicht erlaubt sind.
		/// </summary>
		/// <param name="Input">Der zu prüfende String</param>
		/// <returns>String ohne nichterlaubte Zeichen</returns>
		public static string AdjustPath(string Input) {
			//string invalidChars = Path.GetInvalidFileNameChars().ToString(); <-- warum nimmst du diese nicht her???
			return System.Text.RegularExpressions.Regex.Replace(Input, @"[\\/:*?<>|,#&+=]+", string.Empty);
		}

		public static byte[] FileToByteArray(string filename) {
			byte[] retval = new byte[0];
			FileInfo fi = new FileInfo(filename);
			long length = fi.Length;
			using (FileStream fs = fi.OpenRead()) {
				retval = new byte[length];
				fs.Read(retval, 0, (int)length);
			}
			return retval;
		}

		public static void ByteArrayToFile(byte[] data, string filename) {
			using (FileStream fs = File.Create(filename, data.Length)) {
				fs.Write(data, 0, data.Length);
			}
		}

		public static void Base64ToFile(string base64String, string filepath) {
			byte[] buffer = Convert.FromBase64String(base64String);
			if (buffer != null) {
				ByteArrayToFile(buffer, filepath);
			}
		}

		public static string FileToBase64(string filename) {
			byte[] buffer = FileToByteArray(filename);
			return Convert.ToBase64String(
				buffer,
				Base64FormattingOptions.InsertLineBreaks);
		}

		public static string GetHumanReadableFileSize(long size) {
			if (size < 1024)
				return size.ToString("#,### B");
			else if (size < 10240000)
				return (size / 1024).ToString("###,###,###,###,### KB");
			else
				return (size / 1024000).ToString("###,###,###,### MB");
		}

		public static string GetHumanReadableFileType(string extension) {
			extension = extension.Replace(".", "").ToLower();
			switch (extension) {
				case "pdf":
					return "PDF-Dokument";
				case "doc":
				case "docx":
				case "rtf":
					return "Word-Dokument";
				case "eml":
					return "E-Mail-Nachricht";
				case "xls":
				case "xlsx":
				case "ods":
					return "Excel-Dokument";
				case "vcf":
					return "Visitenkarte";
				case "jpg":
				case "jpeg":
				case "png":
				case "gif":
				case "tif":
					return "Bild-Datei";
			}
			return "unbekanntes Dokument";
		}
		public static string GetMimeTypeFromExtension(string extension) {
			extension = extension.Replace(".", "").ToLower();
			switch (extension) {
				case "pdf":
					return "application/pdf";
				case "doc":
					return "application/msword";
				case "docx":
				case "odt":
					return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				case "rtf":
					return "application/rtf";
				case "xls":
					return "application/msexcel";
				case "xlsx":
				case "ods":
					return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				case "jpg":
				case "jpeg":
					return "image/jpeg";
				case "png":
					return "image/png";
				case "gif":
					return "image/gif";
				case "tif":
					return "image/tiff";
			}
			return "application/octet-stream";
		}

		/// <summary>
		/// Prüft, ob es sich um eine Image-Datei handelt.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsImageFile(string filePath) {
			FileInfo fi = new FileInfo(filePath);
			if (fi.Exists) {
				string extension = fi.Extension.ToLower();
				return IsImageFileExtension(extension);
			}
			return false;
		}
		/// <summary>
		/// Prüft, ob es sich um eine Image-Extension handelt.
		/// </summary>
		/// <param name="extension">Wichtig! Immer den Punkt mit angeben!! (z.B. ".pdf" oder ".docx")</param>
		/// <returns></returns>
		public static bool IsImageFileExtension(string extension) {
			extension = extension.ToLower();
			if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".tif")
				return true;
			return false;
		}

		public static bool WaitForFileUnlocked(string filename, int timeoutInSeconds) {
			DateTime fileReceived = DateTime.Now;
			while (true) {
				int retval = IsFileUnlocked(filename, FileAccess.Read);
				if (retval == 1) {
					return true;
				} else if (retval == -1)
					return false;
				// Calculate the elapsed time and stop if the maximum retry
				// period has been reached.
				TimeSpan timeElapsed = DateTime.Now - fileReceived;
				if (timeElapsed.TotalSeconds > timeoutInSeconds) {
					return false;
				}
				System.Threading.Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Prüft, ob eine Datei im Lesemodus geöffnet werden kann. Rückgabe
		/// 1 = Datei kann geöffnet werden
		/// -1 = Datei wurde nicht gefunden
		/// 0 = Datei ist in verwendung
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static int IsFileUnlocked(string filename, System.IO.FileAccess mode) {
			// If the file can be opened for exclusive access it means that the file
			// is no longer locked by another process.
			try {
				using (FileStream inputStream = File.Open(filename, FileMode.Open,
							mode,
							FileShare.None)) {
					return 1;
				}
			} catch (FileNotFoundException fnfe) {
				return -1;
			} catch (IOException ioe) {
				return 0;
			}
		}

		/// <summary>
		/// Gibt die Größe eines Ordners zurück!
		/// </summary>
		/// <param name="sourceDir">Pfad des Ordners</param>
		/// <param name="recurse">Legt fest, ob die größe von den Unterordnern dazugerechnet werden soll oder ob sie ignoriert werden sollen</param>
		/// <returns>Die Größe des Ordners in Byte</returns>
		public static long GetDirectorySize(string sourceDir, bool recurse) {
			long size = 0;
			string[] fileEntries = Directory.GetFiles(sourceDir);

			foreach (string fileName in fileEntries) {
				try {
					Interlocked.Add(ref size, (new FileInfo(fileName)).Length);
				} catch {
					//hier tritt eine Exception auf wenn genau in dem Zeitraum von Directory.GetFiles() bis hier her eine datei gelöscht wird
				}
			}

			if (recurse) {
				string[] subdirEntries = Directory.GetDirectories(sourceDir);

				Parallel.For<long>(0, subdirEntries.Length, () => 0, (i, loop, subtotal) => {
					if ((File.GetAttributes(subdirEntries[i]) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint) {
						subtotal += GetDirectorySize(subdirEntries[i], true);
						return subtotal;
					}
					return 0;
				},
					(x) => Interlocked.Add(ref size, x)
				);
			}
			return size;
		}
		/// <summary>
		/// Wenn es sich um einen Ordner handelt wird true zurückgegeben wenn es eine File ist dann false
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsFolder(string path) {
			FileAttributes attr = File.GetAttributes(path);
			if (attr.HasFlag(FileAttributes.Directory))
				return true;
			else
				return false;
		}


	}
}
