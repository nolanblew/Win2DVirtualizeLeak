# Win2DVirtualizeLeak
This shows an example of how the Win2D (for UWP on .NET) leaks memory from a UWP app.

Below you can see a chart of what entering and exiting the page with a normal Win2D Canvas looks like (with the red tics being GC):

And with the virtualized canvas:


As you can see, the Virtualized canvas takes much longer to release any memory, and never seems to release all of it
(still holding about +150MB of memory at the end), while the non-virtualized canvas is only holding about +4MB of memory at the end.
