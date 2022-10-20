# Setting Up Software Visualizer

## Prerequisites
* Unity 2021.3.8f1
    * You may want to download Unity Hub first [here](https://unity3d.com/get-unity/download)

## Setting Up the Environment
1. Clone the repo recursively
    ```sh
    git clone git@github.com:CG4002-B3/software_visualizer.git --recurse-submodules -b feat/vuforia_setup
    ```
2. After Unity finishes importing all the assets,
    * Go to Project Settings -> Player. In Android tab, expand Other Settings. Look for Script Compilation -> Scripting Define Symbols, and add `SSL` field.
    * In Unity, configure `Image Target` object as the screenshot below: ![Image Target Configuration](/docs_assets/ImageTargetSettings.png?raw=true "Image Target Configuration")
