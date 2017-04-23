// <copyright file="Program.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks
{
    using BenchmarkDotNet.Running;
    
    using ImageSharp.Formats;
    using System.Reflection;

    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The arguments to pass to the program.
        /// </param>
        public static void Main(string[] args)
        {
            var decode = new Image.DecodePng();
            decode.ReadImages();
            decode.PngCore();
            new BenchmarkSwitcher(typeof(Program).GetTypeInfo().Assembly).Run(args);
        }
    }
}
