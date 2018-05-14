Here are some clues to how to get the cube orientation to be correct.
Start the device vertical when the phone. 

The transformation is correct as it stands.:


The first part getting the cube to rotate with the device works.  I was able to
transfer the mobile build to my kindle Fire and run it from the kindle.  That all
worked.  There is a movie of this in the directory.  


Using an HDFire as a Android Device
-----------------------------------

The Java set up is as described in the environment set-up. Android studio needs to be
set up for each user on the computer so it should be installed for the user that is
doing the work, not for the administrator and not one install for all.  I have an
HDFIre 10.  The API version of this device is 5.1 or API version 22, so I built and
deployed to that version (unity player settings. In Android Studio I
updated all the components when prompted.  I did not install the virtual environment.
In the end in Android Studio the default project says synced successfully and it
build.  It is important not to install build tools for the latest version as that
causes Unity not to build (not sure why). In order to communicate with  an Amazon HD fire, one has
to follow Amazon's instructions on Connecting to Fire through Android Debug Bridge
(ADB).  One has to install a new USB driver.  One can then test whether the kindle

Here is the link:
https://developer.amazon.com/docs/fire-tablets/ft-set-up-your-kindle-fire-tablet-for-testing.html

communicates with the computer or not.  With a USB cable connected the serial number
should show up.  Through ADB, one can also turn off the USB
and send the application wirelessly through the network connection.  The USB transfer
is faster though.  On the device one should in Developer options Enable ADB, so the
kindle works for developers.  Once it is installed it needs to be stopped and
restarted. Hitting the square, one can stop the AP and hitting the AP, one can
uninstall.   Unity essentially calls the Android SDK so it only reports errors.
If successful during build, then you will see transfer APK to "serial number of
device.

ADB has nice tools for debugging that can be used.  
For example adb logcat -s Unity PackageManager dalvikvm DEBUG
With a USB cable connected this gives the debug log and unity errors.
adb logcat -c will clear the log.

Android studio can run and stop the apk file. This no longer works. The debug
messages are different when installed through Android studio.  I think not all the
necessary libraries are installed in my setup.  It is best to build and run through
unity. What I find useful
is to build the PC version as a stand alone and leave it running.  Build the Android
version and use Android Studio to run and stop the app. The adb logcat -s line shows
all the debug errors.

What I found critical was not to add Build Tools for the latest API version.  Gradle
would not compile to android then.  My HDFIRE uses the android API 22 so I built for
that version.  I uninstalled all build tools and compile and deploy worked. 

Also I think you can't deploy this without doing so with unity.  I think libraries
are also used in addition to the apk.  Every time I deployed successfully I did it in
Unity.


Using Vuforia
-------------

This basically worked as advertised.  I made the images larger so that when the
mobile version ran on the PC, the target image filled up the frame.  Similarly for
the PC.  The provided image, for some reason, would not download to the mobile device
and I was getting an error-- image not found.  The Behaviour tracking status went
from unknown and would not trigger state changes.  I went back to my first version
and just added the network stuff and that was no different.  I changed the image to
Astronaut, made sure it filled up the scene.  I picked the astronaut because the
debug log said that image made it to the target. With that I was able to see on
Change Status go from track to untracked to extended.


