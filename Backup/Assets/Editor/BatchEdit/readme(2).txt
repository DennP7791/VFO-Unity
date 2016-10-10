If you are reading this it means that you have installed Batch Edit and Batch Edit is running. 
If you now select two or more GameObjects and edit a field in either the Game Object or in one of it's components this change will
be applied to all selected GameObjects.

Notes:

1. Activator.cs overrides the default Transform editor. If you are using another system that overrides the default Transform editor you need
to move the code in Activator.cs to that file.

2. Editing array sizes will not be applied to all selected Game Objects. 

3. Editing properties in materials will not be applied to all selected Game Objects.