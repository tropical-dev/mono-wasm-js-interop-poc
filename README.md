## Install WASM mono SDK 

Download latest Mono WASM SDK built [from Mono CI](https://jenkins.mono-project.com/job/test-mono-mainline-wasm/label=ubuntu-1804-amd64/lastSuccessfulBuild/Azure/)

Detailed explanation at [Mono project site.](https://github.com/mono/mono/blob/master/sdks/wasm/docs/getting-started/obtain-wasm-sdk.md)

## Set WASM_SDK path to where you unziped the SDK 
```bash 
export WASM_SDK=/Users/cab/Dev/WebAssembly/mono/mono-wasm-ad03563fb46
```

## Build command to compile DLL's
```bash
csc /target:library -out:sample.dll /noconfig /nostdlib /r:$WASM_SDK/wasm-bcl/wasm/mscorlib.dll /r:$WASM_SDK/wasm-bcl/wasm/System.dll /r:$WASM_SDK/wasm-bcl/wasm/System.Core.dll /r:$WASM_SDK/wasm-bcl/wasm/Facades/netstandard.dll /r:$WASM_SDK/wasm-bcl/wasm/System.Net.Http.dll /r:$WASM_SDK/framework/WebAssembly.Bindings.dll /r:$WASM_SDK/framework/WebAssembly.Net.Http.dll  /r:$WASM_SDK/framework/WebAssembly.Net.WebSockets.dll /r:$WASM_SDK/wasm-bcl/wasm/Facades/System.Threading.dll  /r:$WASM_SDK/wasm-bcl/wasm/Facades/System.Text.Encoding.dll /r:$WASM_SDK/wasm-bcl/wasm/Facades/System.Net.WebSockets.dll /r:$WASM_SDK/framework/WebAssembly.Net.WebSockets.dll dependency.cs sample.cs wstest.cs WebGatewayMock.cs
```


## To package and publish DLL's as WASM 
```bash
 mono $WASM_SDK/packager.exe --copy=always --out=./publish --asset=./sample.html --asset=server.py sample.dll
```

## To run the Python server in 

```bash
cd publish
python server.py
```

