using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;

[assembly: AssemblyTitle("MatrixScanCountSimpleSample.Android")]
[assembly: AssemblyCompany("Scandit AG")]
[assembly: AssemblyProduct("MatrixScanCountSimpleSample.Android")]
[assembly: AssemblyCopyright("Copyright © Scandit 2023")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
