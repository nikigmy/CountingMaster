#!/bin/bash
# go into right dir
cd Assets/RemoteDependencies
# remove all dlls before updating
rm -rfv *.dll
# download dependencies
sh ../../BuildScripts/Dependencies/LibraryTest.sh $1
#sh ../../BuildScripts/Dependencies/TurnHero.Portal.Models.sh $1
#sh ../../BuildScripts/Dependencies/TurnHero.Game.Models.sh $1