using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using BuildInfo = AlcoholMod.BuildInfo;

[assembly: ComVisible(false)]
[assembly: Guid("5c0dfb41-a109-48e8-9b78-a05c0b4be5c6")]

[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BuildInfo.Author)]
[assembly: AssemblyTrademark(BuildInfo.Company)]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]
[assembly: MelonInfo(typeof(AlcoholMod.AlcoholMod), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonGame("Hinterland", "TheLongDark")]
