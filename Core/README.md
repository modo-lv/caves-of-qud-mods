# Notes

* **Extension methods can't be shared**. When mod assemblies are loaded, their contents are no longer inherently separate, and core functionality that is duplicated across mods creates a duplicate conflict. For classes C# merely issues a warning and picks the right one based on the calling assembly, but extension methods cause an "ambigious method call" exception. 