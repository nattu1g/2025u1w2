mergeInto(LibraryManager.library, {
  SaveToIndexedDB: function (keyPtr, valuePtr) {
    const key = UTF8ToString(keyPtr);
    const value = UTF8ToString(valuePtr);

    const request = indexedDB.open("MyUnityDB", 1);

    request.onupgradeneeded = function (event) {
      const db = event.target.result;
      if (!db.objectStoreNames.contains("data")) {
        db.createObjectStore("data");
      }
    };

    request.onsuccess = function (event) {
      const db = event.target.result;
      const transaction = db.transaction(["data"], "readwrite");
      const store = transaction.objectStore("data");
      store.put(value, key);
    };

    request.onerror = function (event) {
      console.error("IndexedDB Save Error:", event.target.errorCode);
    };
  },

  LoadFromIndexedDB: function (keyPtr, gameObjectNamePtr, callbackMethodPtr) {
    const key = UTF8ToString(keyPtr);
    const gameObjectName = UTF8ToString(gameObjectNamePtr);
    const callbackMethod = UTF8ToString(callbackMethodPtr);

    const request = indexedDB.open("MyUnityDB", 1);

    request.onupgradeneeded = function (event) {
      const db = event.target.result;
      if (!db.objectStoreNames.contains("data")) {
        db.createObjectStore("data");
      }
    };

    request.onsuccess = function (event) {
      const db = event.target.result;
      const transaction = db.transaction(["data"], "readonly");
      const store = transaction.objectStore("data");
      const getRequest = store.get(key);

      getRequest.onsuccess = function () {
        const result = getRequest.result;
        if (result !== undefined) {
          SendMessage(gameObjectName, callbackMethod, result);
        } else {
          SendMessage(gameObjectName, callbackMethod, "");
        }
      };

      getRequest.onerror = function () {
        console.error("Failed to load from IndexedDB");
        SendMessage(gameObjectName, callbackMethod, "");
      };
    };

    request.onerror = function () {
      console.error("IndexedDB open error:", event.target.errorCode);
      SendMessage(gameObjectName, callbackMethod, "");
    };
  },

  DeleteFromIndexedDB: function (keyPtr) {
    const key = UTF8ToString(keyPtr);

    const request = indexedDB.open("MyUnityDB", 1);

    request.onupgradeneeded = function (event) {
      const db = event.target.result;
      if (!db.objectStoreNames.contains("data")) {
        db.createObjectStore("data");
      }
    };

    request.onsuccess = function (event) {
      const db = event.target.result;
      const transaction = db.transaction(["data"], "readwrite");
      const store = transaction.objectStore("data");
      store.delete(key);
    };

    request.onerror = function (event) {
      console.error("IndexedDB delete error:", event.target.errorCode);
    };
  },

  ClearIndexedDB: function () {
    const request = indexedDB.open("MyUnityDB", 1);

    request.onupgradeneeded = function (event) {
      const db = event.target.result;
      if (!db.objectStoreNames.contains("data")) {
        db.createObjectStore("data");
      }
    };

    request.onsuccess = function (event) {
      const db = event.target.result;
      const transaction = db.transaction(["data"], "readwrite");
      const store = transaction.objectStore("data");
      store.clear();
    };

    request.onerror = function (event) {
      console.error("IndexedDB clear error:", event.target.errorCode);
    };
  }
});
