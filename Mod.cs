using UnityEngine;
using BepInEx;
using HarmonyLib;

[BepInPlugin("com.yourname.catwoman", "Catwoman Mod", "1.0.0")]
public class CatwomanMod : BaseUnityPlugin
{
    public static Harmony Harmony;

    void Awake()
    {
        Harmony = new Harmony("com.yourname.catwoman");
        Harmony.PatchAll();
        Logger.LogInfo("Catwoman Mod Loaded!");
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
public static class CatwomanKillCooldown
{
    static void Postfix(PlayerControl __instance)
    {
        if (__instance == PlayerControl.LocalPlayer)
        {
            bool isCatwoman = true;
            if (isCatwoman)
            {
                __instance.killTimer = 5f; 
            }
        }
    }
}

public static class CatwomanAbilities
{
    public static void Prowl(PlayerControl player)
    {
        player.myRend.material.color = new Color(1f, 1f, 1f, 0.3f);
    }

    public static void EndProwl(PlayerControl player)
    {
        player.myRend.material.color = Color.white;
    }
}
name: Build Catwoman Mod
on:
  push:
    branches: [ main ]
jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.0.x
    - name: Build Mod
      run: dotnet build --configuration Release
    - name: Upload Artifact
      uses: actions/upload-artifact@v3
      with:
        name: Catwoman-Mod-DLL
        path: bin/Release/**/*.dll
