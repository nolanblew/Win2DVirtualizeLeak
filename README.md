# Win2DVirtualizeLeak
This shows an example of how the Win2D (for UWP on .NET) leaks memory from a UWP app.

Below you can see a chart of what entering and exiting the page with a normal Win2D Canvas looks like (with the red tics being GC):
![image](https://user-images.githubusercontent.com/1565705/31200490-fdfe5dc2-a90f-11e7-9bc5-f1a155055056.png)

And with the virtualized canvas:
![capture](https://user-images.githubusercontent.com/1565705/31200472-ef137e32-a90f-11e7-877e-08f34f0f6f1c.PNG)

As you can see, the Virtualized canvas takes much longer to release any memory, and never seems to release all of it
(still holding about +150MB of memory at the end), while the non-virtualized canvas is only holding about +4MB of memory at the end.
