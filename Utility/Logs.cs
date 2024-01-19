using System.Text;

namespace Utility
{
    public class Logs
    {
        public void WriteLogs( string logs, string name)
        {
            try
            {
                string _path = @"D:\SpiceJet\" + DateTime.Now.ToString("ddMMMyyyy");
                if (!Directory.Exists(_path) || !File.Exists(_path))
                {
                    System.IO.Directory.CreateDirectory(_path);
                }
                File.WriteAllText(_path + "\\" + name + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", logs);
            }
            catch (Exception ex)
            {
            }

        }

        public void WriteLogsR(string logs, string name,string AirLine)
        {
            try
            {
                string _path = @"D:\"+ AirLine + @"\" + DateTime.Now.ToString("ddMMMyyyy");
                if (!Directory.Exists(_path) || !File.Exists(_path))
                {
                    System.IO.Directory.CreateDirectory(_path);
                }
                File.WriteAllText(_path + "\\" + name + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", logs);
            }
            catch (Exception ex)
            {
            }

        }

    }
}