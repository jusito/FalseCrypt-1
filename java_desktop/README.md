# FalseCrypt: Java desktop

## ToDo
- Typisierung wonach?
- Leer lassen, "no java bug", x für: bug 6/7, 12, 14, 15/16, 20, so nicht direkt in java möglich?
- bug 10/17, Not disposing IDisposable -> pbeKeySpec.clearPassword() oder/und secretKey.destroy() benötigt? (clearPassword glaube ich ja, secretKey meine ich sollte nur so früh wie möglich gelöscht werden, bin mir unsicher)


## known bugs

### FalseCrypt.App.View
|bug number|type|linenumber|line|description|
|----------|----|----------|----|-----------|
|01||[Line 186-211](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/App/View.java#L186-L211) |`keyData = WeakPasswordDerivation.DerivePassword(password);`|Key derivation should not be performed outside a foreach block that is using its return value. Otherwise all operations in "encrypt directory" have the same encryption key|

### FalseCrypt.Crypto.WeakCryptoConfig
|bug number|type|linenumber|line|description|
|----------|----|----------|----|-----------|
|02||[Line 10](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakCryptoConfig.java#L10)|`public final static String Password = "482c811da5d5b4bc6d497ffa98491e38";`|A password, even it's hash, should not be hardcoded so it's easy to get it via decompilation|

### FalseCrypt.Crypto.WeakKeyGenerator
|bug number|type|linenumber|line|description|
|----------|----|----------|----|-----------|
|03||[Line 7](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakKeyGenerator.java#L7)|`private final static int SecretSeed = 852641973;`|Constant seed makes it attackers easy to recreate the pseudo random sequence|
|04||[Line 13](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakKeyGenerator.java#L13)|`Random RandomGenerator = new Random(SecretSeed);`|A property with object initializer means that with each access a new instance of random with the same seed will be created.|
|05||[Line 16](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakKeyGenerator.java#L16)|`RandomGenerator.nextBytes(byteArray);`|System.Random is not cryptographically secure|

### FalseCrypt.Crypto.WeakPasswordDerivation
|bug number|type|linenumber|line|description|
|----------|----|----------|----|-----------|
|06|||||
|07|||||
|08||[Line 40](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakPasswordDerivation.java#L40)|`PBEKeySpec pbeKeySpec = new PBEKeySpec(password.toCharArray(), salt, WeakCryptoConfig.IterationCount, WeakCryptoConfig.KeySizeBytes * 8);`|Iteration count is too low|
|09||[Line 40](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakPasswordDerivation.java#L40)|`PBEKeySpec pbeKeySpec = new PBEKeySpec(password.toCharArray(), salt, WeakCryptoConfig.IterationCount, WeakCryptoConfig.KeySizeBytes * 8);`|Salt size is constant 8 bytes long and hence to short|
|10||[Line 40-41](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakPasswordDerivation.java#L40-L41)|`PBEKeySpec pbeKeySpec = new PBEKeySpec(password.toCharArray(), salt, WeakCryptoConfig.IterationCount, WeakCryptoConfig.KeySizeBytes * 8);<br>SecretKey secretKey = factory.generateSecret(pbeKeySpec);`|pbeKeySpec.clearPassword() & secretKey.destroy() never used|
|11||[Line 46](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakPasswordDerivation.java#L46)|`String strKey = new String(key);`|Password as a string allows memory dump attacks|

### FalseCrypt.Crypto.WeakSymmetricEncryption
|bug number|type|linenumber|line|description|
|----------|----|----------|----|-----------|
|12|||||
|13||[Line 34](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L34)|`Cipher des = Cipher.getInstance("DES/ECB/PKCS5Padding");`|Weak operation mode/padding|
|14|||||
|15|||||
|16|||||
|17||[Line 40](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L40)|`new SecretKeySpec(key, "DES"));`|secretKey.destroy() never used|
|18||[Line 43](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L48)|`final CipherOutputStream cos = new CipherOutputStream(os, des);`|Not closing Closable|
|19||[Line 51](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L51)|`cipherText = os.toByteArray();`|Array with clear text content should be destroyed after a encryption. Comment below shows how|
|20|||||
|21||[Line 76](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L76)|`Cipher des = Cipher.getInstance("DES/ECB/PKCS5Padding");`|Weak operation mode / padding|
|22||[Line 81](https://github.com/jusito/FalseCrypt-1/blob/develop/java_desktop/src/main/java/FalseCrypt/Crypto/WeakSymmetricEncryption.java#L81)|`final CipherInputStream cis = new CipherInputStream(is, des);`|Not closing Closable|