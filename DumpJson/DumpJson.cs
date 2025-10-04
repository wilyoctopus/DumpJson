using System;
using System.Diagnostics;
using System.Text.Json;

namespace DumpJson
{
    /// <summary>
    /// Provides extension methods for dumping objects to output.
    /// </summary>
    public static class DumpJsonExtensions
    {
        /// <summary>
        /// Serializes and outputs the object to console and/or debug output, then returns the original object.
        /// </summary>
        /// <typeparam name="T">The type of the object to dump.</typeparam>
        /// <param name="obj">The object to dump.</param>
        /// <param name="label">An optional label to display before the serialized output.</param>
        /// <returns>The original object, allowing for method chaining.</returns>
        public static T Dump<T>(this T obj, string label = null) => Output.Write(obj, label);
    }

    /// <summary>
    /// Contains configuration settings for the dump functionality.
    /// </summary>
    public class DumpJsonSettings
    {
        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// Gets or sets the JSON serializer options used when serializing objects.
        /// </summary>
        /// <value>The JSON serializer options. Default is indented formatting.</value>
        /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
        public JsonSerializerOptions JsonSerializerOptions
        {
            get => _jsonSerializerOptions;
            set => _jsonSerializerOptions = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a value indicating whether to write output to the console.
        /// </summary>
        /// <value>
        /// <c>true</c> to write to console; <c>false</c> to skip console output; <c>null</c> to auto-detect console availability.
        /// </value>
        public bool? WriteToConsoleOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write output to the debug output window.
        /// </summary>
        /// <value>
        /// <c>true</c> to write to debug output; <c>false</c> to skip debug output; <c>null</c> to auto-detect debugger attachment.
        /// </value>
        public bool? WriteToDebugOutput { get; set; }
    }

    /// <summary>
    /// Provides global settings for dumping objects to output.
    /// </summary>
    public static class DumpJson
    {
        /// <summary>
        /// Gets the global dump settings instance.
        /// </summary>
        /// <value>The dump settings that control serialization and output behavior.</value>
        public static DumpJsonSettings Settings { get; } = new DumpJsonSettings();
    }

    internal static class Output
    {
        private static bool _debugOutputAvailable => Debugger.IsAttached;
        private static readonly bool _consoleOutputAvailable = ConsoleAvailable();
        private static readonly bool _shouldWriteToDebug = DumpJson.Settings.WriteToDebugOutput ?? _debugOutputAvailable;
        private static readonly bool _shouldWriteToConsole = DumpJson.Settings.WriteToConsoleOutput ?? _consoleOutputAvailable;
        
        internal static T Write<T>(T obj, string label = null)
        {   
            if (!_shouldWriteToConsole && !_shouldWriteToDebug) return obj;

            var json = JsonSerializer.Serialize(obj, DumpJson.Settings.JsonSerializerOptions);
            WriteLabel(label);
            WriteJson(json);
            return obj;
        }

        internal static void WriteLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return;
            if (_shouldWriteToDebug) Debug.WriteLine(label);
            if (_shouldWriteToConsole) Console.WriteLine(label);
        }

        internal static void WriteJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            if (_shouldWriteToDebug) Debug.WriteLine(json);
            if (_shouldWriteToConsole) Console.WriteLine(json);
        }

        private static bool ConsoleAvailable()
        {
            try { _ = Console.WindowHeight; return true; }
            catch { return false; }
        }
    }
}