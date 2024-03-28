# SQLite Key-Value Store for Unity
[![openupm](https://img.shields.io/npm/v/com.gilzoide.key-value-store.sqlite?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.gilzoide.key-value-store.sqlite/)

[Key-Value Store](https://github.com/gilzoide/unity-key-value-store) implementation backed by the [SQLite](https://sqlite.org) database engine.


## Features
- Automatic management of transactions, optimizing throughput.
  Write operations are commited automatically once per frame.
- Custom serialization of complex objects (class/struct types), using Key-Value Store's serialization system
- Supports running `PRAGMA` statements for customization of the database
- Supports running `VACUUM` statements manually


## Dependencies
- [Key-Value Store](https://github.com/gilzoide/unity-key-value-store): interface used by this implementation, which also provides custom object serialization out of the box.
- [SQLite-net](https://github.com/gilzoide/unity-sqlite-net): library for managing SQLite databases


## How to install
Either:
- Use the [openupm registry](https://openupm.com/) and install this package using the [openupm-cli](https://github.com/openupm/openupm-cli):
  ```
  openupm add com.gilzoide.key-value-store.sqlite
  ```
- Install using the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with the following URL:
  ```
  https://github.com/gilzoide/unity-key-value-store-sqlite.git#1.0.0-preview1
  ```
- Clone this repository or download a snapshot of it directly inside your project's `Assets` or `Packages` folder.


## Basic usage
```cs
using Gilzoide.KeyValueStore.Sqlite;
using UnityEngine;

// 1. Instantiate a SqliteKeyValueStore with the desired path
var kvs = new SqliteKeyValueStore(Application.persistentDataPath + "/MySaveFile.db");


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


// 3. Dispose of the SqliteKeyValueStore when done
// This ensures the database connection gets closed correctly
kvs.Dispose();
```