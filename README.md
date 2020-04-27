# GearSwapUtility

A helper utility program for creating GearSwap sets visually

![GearSwapUtility Screenshot](https://github.com/maverickdfz/GearSwapUtility/raw/master/screenshot.png "GearSwapUtility Screenshot")

You will need to configure the path to your windower folder and a folder containing equipment icon pngs by clicking Edit > Preferences...

32x32 equipment icons can be downloaded from the equipviewer repository here: [icons.7z](https://github.com/ProjectTako/ffxi-addons/blob/master/equipviewerv2/windower/icons.7z)

## File menu

Save progress to a JSON file for later

Load from a JSON to continue working on sets, for example changing the body piece when you upgrade

Export creates a lua file with the sets to be used with a job specific lua file

## Usage

Right click in the left region to add sets

### Based on the example provided in [GearSwapUtility-lua](https://github.com/maverickdfz/GearSwapUtility-lua)

Use lower case set names for the various stages

Use english capitalized set names when defining sets for specific actions

Set names can be (but are not limited to):

```
precast
    FC
    JA
    WS
    RA
midcast
    RA
    Pet
idle
resting
engaged
defense
buff
```

Examples of more specific set names:

```
precast
    Step
        Feather Step
    WS
        Chant du Cygne
midcast
    FC
        Cure
```

## Examples

An example configuration for RDM can be found here: [GearSwapUtility-lua](https://github.com/maverickdfz/GearSwapUtility-lua)

In this example the RDM.lua file uses the RDM-lua

Load the included JSON file and have a dig around