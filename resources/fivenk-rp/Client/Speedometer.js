var speedometerBrowser = null;
var wasInVehicle = false;

API.onResourceStart.connect(function() {
    // In 2560x1440 I want it placed at (450, 1300) so normalize:
    var res = API.getScreenResolution();
    var xPos = 450 * (res.Width / 2560.0);
    var yPos = 1300 * (res.Height / 1440.0);
    speedometerBrowser = API.createCefBrowser(200, 200);
    API.setCefBrowserPosition(speedometerBrowser, xPos, yPos);
    API.setCefBrowserHeadless(speedometerBrowser, true);
    API.waitUntilCefBrowserInit(speedometerBrowser);
    API.loadPageCefBrowser(speedometerBrowser, "Client/CEF/speedometer.html");
});

API.onUpdate.connect(function() {
    var player = API.getLocalPlayer();
    var isPlayerInVehicle = API.isPlayerInAnyVehicle(player);

    if (speedometerBrowser != null) {
        if (isPlayerInVehicle) {
            var car = API.getPlayerVehicle(player);
            var velocity = API.getEntityVelocity(car);
            var speed = Math.sqrt(
                velocity.X * velocity.X +
                velocity.Y * velocity.Y +
                velocity.Z * velocity.Z
                );

            // m/s to km/h
            speedometerBrowser.call("updateSpeed", speed * 3.6);

            // We only hide/show the speedometer if the player left/entered
            // a vehicle since the last frame rather than every frame
            if (!wasInVehicle) {
                API.setCefBrowserHeadless(speedometerBrowser, false);
            }
        } else if (wasInVehicle) {
            API.setCefBrowserHeadless(speedometerBrowser, true);
        }
    }
    wasInVehicle = isPlayerInVehicle;
});

API.onResourceStop.connect(function() {
    if (speedometerBrowser != null) {
        API.destroyCefBrowser(speedometerBrowser);
    }
});