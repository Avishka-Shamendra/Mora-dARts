# Mora-dARts

Introducing _Mora dARts_, a **single-player AR dart game** for mobile devices created using the Unity game engine and the features of AR Foundation. 

## Setting Up the Repo

1. Clone this repo (recommended to use github desktop)
2. Open Unity Hub 
3. Click drop down near "Open" button
4. Click "Add project from disk"
5. Then navigate to place where you cloned the repo
6. Click and open the project foder "Mora dARts" (not Mora-dARTs folder)
7. You will only see the changes once you go to "Assets" section in Unity and click "Scenes" folder.

## Build the App
1. Navigate to `Files > Build Settings` and there select Android as the platform.
2. Click on build and slect the path where the APK should be created.

## Device Requirements 
In order to successfully install the app, the following device requirements must be met:
* The device or emulator must be running Android 7.0 (API level 24) or higher
AR Core support must be available. 
* You can check whether your device supports AR core from visiting https://developers.google.com/ar/devices.
* A high-quality rear-facing camera is necessary.
* The device must have a touch-sensitive screen.
* At least 100 MB of free space on the device.

## Installation Steps
* To download the Mora Darts APK onto your Android device, begin by visiting https://dms.uom.lk/s/8kwHEnkwNPtREKq
* Once the APK has been downloaded, proceed to install the application on your mobile device. 
* Keep in mind that a security warning may be displayed on your phone due to the app not being downloaded from the Play Store. In this case, simply acknowledge the warning and continue with the installation process.

## Game Rules
* The game starts with the player having 501 points and 50 darts.
* Each dart is thrown one at a time.
* The player's points will be reduced based on where the dart hits the board. The relevant point values for different areas are indicated in the below figure.
* The player must be at least 0.8m away from the dartboard for their points to count.
* If the player reaches exactly 0 points with the given number of darts, they win the game, and the score will be the remaining number of darts. (By "exactly 0," we mean that if the remaining points are 30, any points from a dart throw above 30 will be disregarded.)
* If the player does not reach exactly 0 points with the given number of darts, they lose the game.

## How to Play
Here is a step-by-step guide on how to play the game:
* Launch the app and select "START".
* When the camera opens from within the app, point it towards a vertical plane (such as a wall or door) where you want to place the dartboard. Note that you must be in a well-lit place, and you may need to allow permission for the app to use your device's camera.
* Keep pointing towards the vertical plane until you see a view similar to the image below. You should also see a black placement indicator on the plane where the dartboard will be placed.
* Tap once on the screen. You should now see the dartboard projected onto the plane, along with a dart in front of it. If the dartboard is not in the right orientation, you can drag two fingers on the screen to rotate it as needed.
* You are now ready to start playing darts. You should see the points value on the top left, the number of darts you have left on the top right, and the distance between you and the board on the bottom left.
* To throw a dart, tap the dart once. The dart will be thrown towards the board, and once it collides with the board, you will see the points you scored from that dart pop up and the total points value getting updated.
* If you are too close to throw, a "Too close" message will appear when you try to throw a dart. 
* If your dart throw scores more than the required remaining points, you will see the "Too much" message pop up.
* Once you win or lose the game, you will see the end game screen. Here, you can see your score and the highest score. You can press "Play Again" to start a new game.

