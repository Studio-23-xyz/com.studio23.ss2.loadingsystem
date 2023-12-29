<h1 align="center">Loading System</h1>
<p align="center">
<a href="https://openupm.com/packages/com.studio23.ss2.loadingsystem/"><img src="https://img.shields.io/npm/v/com.studio23.ss2.loadingsystem?label=openupm&amp;registry_uri=https://package.openupm.com" /></a>
</p>

Loading system is used for to load scene, create different type of loading screen between scene switches, create different type of hints to show between loading screen. 

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
- [Getting Started](#Getting-Started)
- [Using the SceneLoading System](#Using the SceneLoading System)
- [Using the AbstractLoadingScreen UI script](#Using the AbstractLoadingScreen UI script)

## Installation

### Install via Git URL

You can also use the "Install from Git URL" option from Unity Package Manager to install the package.
```
https://github.com/Studio-23-xyz/com.studio23.ss2.loadingsystem.git#upm
```

## Usage

### Getting Started

To start using the Loading System. You need to take a few setup stepts

1. Click on the 'Studio-23' available on the top navigation bar and navigate to LoadingSystem > Generate Scene Data to create a SceneTable script that contains all of the scenes in the builds.

2. New hints can be created from Create > Studio-23 > LoadingSystem > Loading Screen TextTable.

3. Click on the 'Studio-23' available on the top navigation bar and navigate to LoadingSystem > Generate Hint Styling Data to create a new stryle for the hints to show.


### Using the SceneLoading System

1. Create an empty gameobject. 

2. Attach The SceneLoadingSystem Script to it. 

3. Assign one of the created loading screen prefab to use in SceneLoadingSystem script Loading Screen Prefab slot.

4. Simply call this following method to load scene.


### Using the AbstractLoadingScreen UI script

1. AbstractLoadingScreen UI script is responsible for showing ui during loading scene and it should be attached to every loading screen prefab.

2. We can modify the ui content from this script.

3. We can assign hints and hint style from the inspector.

4. We can tweak hint duration, crossfade duration and need for key press from the inspector.

5. We can assign mutiple images in the Background Images slot for crossfade effect.


```Csharp
SceneLoadingSystem.Instance.LoadScene(string sceneName);
```

