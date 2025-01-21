using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ChawakornModelPredictionLibrary
{
    public class PythonModelPrediction
    {
        private string pythonExecutablePath = null;
        private string pythonScriptPath = null;

        public PythonModelPrediction(string pythonExecutablePath, string pythonScriptPath)
        {
            if (string.IsNullOrEmpty(pythonExecutablePath))
            {
                throw new ArgumentNullException(nameof(pythonExecutablePath), "Python executable path cannot be null or empty.");
            }

            if (!File.Exists(pythonExecutablePath))
            {
                throw new FileNotFoundException("Python executable file not found.", pythonExecutablePath);
            }

            if (string.IsNullOrEmpty(pythonScriptPath))
            {
                throw new ArgumentNullException(nameof(pythonScriptPath), "Python script path cannot be null or empty.");
            }

            if (!File.Exists(pythonScriptPath))
            {
                throw new FileNotFoundException("Python script file not found.", pythonScriptPath);
            }

            this.pythonExecutablePath = pythonExecutablePath;
            this.pythonScriptPath = pythonScriptPath;
        }
        public async Task StreamAsync(string input, Action<int> output)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonExecutablePath,
                Arguments = pythonScriptPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {

                try
                {
                    process.Start();
                    await process.StandardInput.WriteLineAsync(input);
                    process.StandardInput.Close();
                    string result = await process.StandardOutput.ReadToEndAsync();
                    string errorOutput = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    if (!string.IsNullOrEmpty(errorOutput))
                    {
                        throw new PythonProcessException($"Python script error:{errorOutput} ");
                    }
                    if (int.TryParse(result.Trim(), out int value))
                    {
                        output(value);
                    }
                    else
                    {
                        throw new PythonOutputFormatException($"Invalid output: {result}");
                    }

                }
                catch (PythonProcessException ex)
                {
                    output(-1);
                    throw ex;
                }
                catch (PythonOutputFormatException ex)
                {
                    output(-2);
                    throw ex;
                }
                catch (Exception ex)
                {
                    output(-3);
                    throw new PythonProcessException("An error occurred while running python process", ex);
                }
                finally
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
            }
        }
        public class PythonProcessException : Exception
        {
            public PythonProcessException(string message) : base(message) { }
            public PythonProcessException(string message, Exception innerException) : base(message, innerException) { }

        }
        public class PythonOutputFormatException : Exception
        {
            public PythonOutputFormatException(string message) : base(message) { }
        }

    }
}