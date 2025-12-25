#!/bin/bash
echo ":rocket: Define parameters"
IOS_SDK_VERSION="26.0"
SWIFT_PROJECT_NAME="BarkoderFrameworkProxy"
SWIFT_PROJECT_PATH="$SWIFT_PROJECT_NAME/$SWIFT_PROJECT_NAME.xcodeproj"
SWIFT_BUILD_PATH="$SWIFT_PROJECT_NAME/build"
SWIFT_XCFRAMEWORK_PATH="$SWIFT_BUILD_PATH/Release-fat" # NEW FOLDER FOR XCFRAMEWORKS
XAMARIN_BINDING_PATH="Xamarin/SwiftFrameworkProxy.Binding"
echo ":broom: Cleaning previous build..."
rm -Rf "$SWIFT_BUILD_PATH"
echo ":hammer: Building iOS frameworks for device (arm64 only)"
# :white_check_mark: Build for **device** (`arm64` only)
xcodebuild -scheme "$SWIFT_PROJECT_NAME" -sdk iphoneos$IOS_SDK_VERSION -project "$SWIFT_PROJECT_PATH" -configuration Release \
ARCHS="arm64" EXCLUDED_ARCHS="x86_64 i386 arm64e" \
ONLY_ACTIVE_ARCH=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
-derivedDataPath "$SWIFT_BUILD_PATH"
# :white_check_mark: Ensure the device build exists
if [ ! -d "$SWIFT_BUILD_PATH/Build/Products/Release-iphoneos" ]; then
  echo ":x: Error: Device build failed. No Release-iphoneos directory found."
  exit 1
fi
mkdir -p "$SWIFT_BUILD_PATH/Release-iphoneos"
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphoneos/$SWIFT_PROJECT_NAME.framework" "$SWIFT_BUILD_PATH/Release-iphoneos/"
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphoneos/Barkoder.framework" "$SWIFT_BUILD_PATH/Release-iphoneos/" 2>/dev/null
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphoneos/BarkoderSDK.framework" "$SWIFT_BUILD_PATH/Release-iphoneos/" 2>/dev/null
echo ":hammer: Building iOS frameworks for simulator (x86_64 & arm64)"
# :white_check_mark: Build for **simulator** (`x86_64` and `arm64`)
xcodebuild -scheme "$SWIFT_PROJECT_NAME" -sdk iphonesimulator$IOS_SDK_VERSION -project "$SWIFT_PROJECT_PATH" -configuration Release \
ARCHS="x86_64 arm64" EXCLUDED_ARCHS="arm64e" \
ONLY_ACTIVE_ARCH=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
-derivedDataPath "$SWIFT_BUILD_PATH"
# :white_check_mark: Ensure the simulator build exists
if [ ! -d "$SWIFT_BUILD_PATH/Build/Products/Release-iphonesimulator" ]; then
  echo ":x: Error: Simulator build failed. No Release-iphonesimulator directory found."
  exit 1
fi
mkdir -p "$SWIFT_BUILD_PATH/Release-iphonesimulator"
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphonesimulator/$SWIFT_PROJECT_NAME.framework" "$SWIFT_BUILD_PATH/Release-iphonesimulator/"
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphonesimulator/Barkoder.framework" "$SWIFT_BUILD_PATH/Release-iphonesimulator/" 2>/dev/null
cp -R "$SWIFT_BUILD_PATH/Build/Products/Release-iphonesimulator/BarkoderSDK.framework" "$SWIFT_BUILD_PATH/Release-iphonesimulator/" 2>/dev/null
echo ":package: Creating XCFrameworks Directory"
rm -Rf "$SWIFT_XCFRAMEWORK_PATH"
mkdir -p "$SWIFT_XCFRAMEWORK_PATH"
echo ":link: Generating XCFrameworks"
# :white_check_mark: Generate BarkoderFrameworkProxy.xcframework
if [ -d "$SWIFT_BUILD_PATH/Release-iphoneos/$SWIFT_PROJECT_NAME.framework" ] && [ -d "$SWIFT_BUILD_PATH/Release-iphonesimulator/$SWIFT_PROJECT_NAME.framework" ]; then
  xcodebuild -create-xcframework \
  -framework "$SWIFT_BUILD_PATH/Release-iphoneos/$SWIFT_PROJECT_NAME.framework" \
  -framework "$SWIFT_BUILD_PATH/Release-iphonesimulator/$SWIFT_PROJECT_NAME.framework" \
  -output "$SWIFT_XCFRAMEWORK_PATH/$SWIFT_PROJECT_NAME.xcframework"
fi
# :white_check_mark: Generate Barkoder.xcframework
if [ -d "$SWIFT_BUILD_PATH/Release-iphoneos/Barkoder.framework" ] && [ -d "$SWIFT_BUILD_PATH/Release-iphonesimulator/Barkoder.framework" ]; then
  xcodebuild -create-xcframework \
  -framework "$SWIFT_BUILD_PATH/Release-iphoneos/Barkoder.framework" \
  -framework "$SWIFT_BUILD_PATH/Release-iphonesimulator/Barkoder.framework" \
  -output "$SWIFT_XCFRAMEWORK_PATH/Barkoder.xcframework"
fi
# :white_check_mark: Generate BarkoderSDK.xcframework
if [ -d "$SWIFT_BUILD_PATH/Release-iphoneos/BarkoderSDK.framework" ] && [ -d "$SWIFT_BUILD_PATH/Release-iphonesimulator/BarkoderSDK.framework" ]; then
  xcodebuild -create-xcframework \
  -framework "$SWIFT_BUILD_PATH/Release-iphoneos/BarkoderSDK.framework" \
  -framework "$SWIFT_BUILD_PATH/Release-iphonesimulator/BarkoderSDK.framework" \
  -output "$SWIFT_XCFRAMEWORK_PATH/BarkoderSDK.xcframework"
fi
echo ":mag: Verifying XCFrameworks"
ls -la "$SWIFT_XCFRAMEWORK_PATH"
echo ":outbox_tray: Copying XCFrameworks to output folder"
rm -Rf "$SWIFT_OUTPUT_PATH"
mkdir -p "$SWIFT_OUTPUT_PATH"
cp -Rf "$SWIFT_XCFRAMEWORK_PATH/"* "$SWIFT_OUTPUT_PATH"
echo ":scroll: Generating binding API definition and structs"
sharpie bind --sdk=iphoneos$IOS_SDK_VERSION --output="$SWIFT_OUTPUT_PATH/XamarinApiDef" \
--namespace="BarkoderBinding" --scope="$SWIFT_OUTPUT_PATH/$SWIFT_PROJECT_NAME.xcframework/Headers/" \
"$SWIFT_OUTPUT_PATH/$SWIFT_PROJECT_NAME.xcframework/Headers/$SWIFT_PROJECT_NAME-Swift.h"
echo ":arrows_counterclockwise: Replacing existing metadata with the updated one"
cp -Rf "$SWIFT_OUTPUT_PATH/XamarinApiDef/." "$XAMARIN_BINDING_PATH/"
echo ":white_check_mark: Done! XCFrameworks are now inside the `xcframeworks/` folder!"
