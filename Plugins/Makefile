LINKFLAGS += -shared -framework Foundation -framework Security

macOS/KeyValueStoreAppleKeychain.dylib: CFLAGS += -arch arm64 -arch x86_64
macOS/KeyValueStoreAppleKeychain.dylib: source/KeyValueStoreAppleKeychain.m
	$(CC) -o $@ $< $(CFLAGS) $(LINKFLAGS)