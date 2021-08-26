# Cherry Framework
*The cherry on the cake.*

## Principles
1. **NO** global states.
2. **NO** circular dependencies.
3. **YES** decoupled modules.
4. **YES** layered design.
5. **YES** separation of concerns.

## How to install
Under construction. UPM installation guide will be here later.

## Modules
The module is each folder under the `Assets` directory. Each module can be referenced by any other module, except modules referenced by this module itself. Common modules (with multiple references by others) are allowed, but circular are not.

This project contains following modules:
* `Cherry.Core` — core gameplay ECS framework
* `Cherry.Loading` — common bootstrapping code (entry point, composition root)
* `Cherry.Common` — common utilities (serialization, logging, etc.)
* `Cherry.Magicka` — initial project using frameworks described above
* `Cherry.ThirdParty` — third-party plugins and tools

## How to add module
### Base module
Just add it into the `Assets` directory. And don't forget to add `.asmdef` files to every folder, with `"autoReferenced": false` key specified. It won't let the assembly being referenced in a circular way.

### Editor module
If you want to introduce an editor namespace for any module, please put it on the same level as base module, e.g.:
```
/Assets/
├── Cherry.Core
├── Cherry.Core.Editor
```

### Test module
The same as editor module.
```
/Assets/
├── Cherry.Core
├── Cherry.Core.Editor
├── Cherry.Core.Tests
```