# Apple Keychain Key-Value Store for Unity
[Key-Value Store](https://github.com/gilzoide/unity-key-value-store) implementation backed by Apple platforms' [Keychain Services](https://developer.apple.com/documentation/security/keychain_services).


## Features
- [GenericPasswordKeychainItemKeyValueStore](Runtime/GenericPasswordKeychainItemKeyValueStore.cs): Key-Value Store implementation that stores data into a Generic Password Keychain Item.
  Value data is stored as a `NSDictionary` serialized by `NSKeyedArchiver`.
- Supports Keychain Items synchronizable with iCloud, juse set `IsSynchronizable`


## Dependencies
- [Key-Value Store](https://github.com/gilzoide/unity-key-value-store): interface used by this implementation, which also provides custom object serialization out of the box.


## How to install
Either:
- Install using the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with the following URL:
  ```
  https://github.com/gilzoide/unity-key-value-store-apple-keychain.git#1.0.0-preview1
  ```
- Clone this repository or download a snapshot of it directly inside your project's `Assets` or `Packages` folder.


## Basic usage
```cs
using Gilzoide.KeyValueStore.AppleKeychain;
using UnityEngine;

// 1. Instantiate a GenericPasswordKeychainItemKeyValueStore with the Keychain Item attributes
var keychainItemAttributes = new GenericPasswordKeychainAttributes
{
    Service = Application.identifier,
    Description = "Small secrets used by the game",
    IsSynchronizable = true,  // synchronizable with iCloud
};

var kvs = new GenericPasswordKeychainItemKeyValueStore(keychainItemAttributes);


// 2. Set/Get/Delete values
kvs.SetBool("finishedTutorial", true);
kvs.SetString("username", "gilzoide");

Debug.Log("Checking if values exist: " + kvs.HasKey("username"));
Debug.Log("Getting values: " + kvs.GetInt("username"));
Debug.Log("Getting values with fallback: " + kvs.GetString("username", "default username"));
// Like C# Dictionary, this idiom returns a bool if the key is found
if (kvs.TryGetString("someKey", out string foundValue))
{
    Debug.Log("'someKey' exists: " + foundValue);
}

kvs.DeleteKey("someKey");


// 3. Save the updated data into the keychain
kvs.Save();


// 4. Dispose of the GenericPasswordKeychainItemKeyValueStore when done
// This ensures the native data gets released correctly
kvs.Dispose();
```