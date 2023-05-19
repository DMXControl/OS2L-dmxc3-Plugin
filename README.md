# DMXControl 3 - OS2L Plugin

This plugin implements the OS2L protocol ([os2l.org](http://os2l.org/)) for DMXControl 3.
OS2L allows audio software like VirtualDJ to send messages to DMX software like DMXControl. 
Currently only beat messages are supported within this plugin.

## Install
- Copy the DLL-files to the Kernel plugin directory, i.e. `C:\Program Files (x86)\DMXControl3\Kernel\Plugins`.
- Sometimes it is needed to right-click all DLL-files, select `Properties` and then click `Unblock`.

## Usage
- Enable the plugin in the DMXControl Plugin Management
- The `OS2L Beat` will appear in the Input Assignment

## Notes
- VirtualDJ will only connect to one OS2L Plugin (the first VirtualDJ finds in the local network).
  If DMXControl runs multiple times in the same network, enable this plugin only on the computer where you want VirtualDJ to connect. 
- VirtualDJ will send the beat messages only after you have clicked on any of the DMX buttons with the default settings.
  If you want to change this, open the VirtualDJ settings, select `Options`, search for `os2l` and select `yes` instead of `auto`.
