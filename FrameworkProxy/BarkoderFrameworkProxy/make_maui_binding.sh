#!/bin/bash

rm -r build/

xcodebuild -sdk iphonesimulator17.2 -project BarkoderFrameworkProxy.xcodeproj -configuration Release BUILD_LIBRARY_FOR_DISTRIBUTION=YES EXCLUDED_ARCHS=arm64

xcodebuild -sdk iphoneos17.2 -project BarkoderFrameworkProxy.xcodeproj -configuration Release

cd build

cp -R Release-iphoneos Release-fat

cp -R "Release-iphonesimulator/BarkoderFrameworkProxy.framework/Modules/BarkoderFrameworkProxy.swiftmodule" 
Release-fat/BarkoderFrameworkProxy.framework/Modules/BarkoderFrameworkProxy.swiftmodule/

lipo -create -output Release-fat/BarkoderFrameworkProxy.framework/BarkoderFrameworkProxy Release-iphoneos/BarkoderFrameworkProxy.framework/BarkoderFrameworkProxy 
Release-iphonesimulator/BarkoderFrameworkProxy.framework/BarkoderFrameworkProxy

sharpie bind --sdk=iphoneos17.2 --output="XamarinApiDef" --namespace="BarkoderBinding" --scope="Release-fat/BarkoderFrameworkProxy.framework/Headers/" "Release-fat/BarkoderFrameworkProxy.framework/Headers/BarkoderFrameworkProxy-Swift.h"
