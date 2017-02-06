var loginBrowser = null;
var Camera = null;
var CameraPos = new Vector3(3513.92, 5118.72, 5.76); // Can try to find a cool view later

API.onResourceStart.connect(function() {
    API.triggerServerEvent("player_connected");
    Camera = API.createCamera(CameraPos, new Vector3());
    API.pointCameraAtEntity(Camera, API.getLocalPlayer(), new Vector3());
    API.setActiveCamera(Camera);
});

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
    case "display_login":
        OnDisplayLoginEvent();
        break;
    case "display_signup":
        OnDisplaySignupEvent();
        break;
    case "fail_login":
        loginBrowser.call("EnableSubmitButton", true);
        // TODO#41 - Display popup that login failed args[0] is reason
        API.sendChatMessage("~r~ERROR:~w~ Login failed: " + args[0]);
        break;
    case "fail_signup":
        loginBrowser.call("EnableSubmitButton", true);
        // TODO#41 - Display popup that signup failed args[0] is reason
        API.sendChatMessage("~r~ERROR:~w~ Registration failed: " + args[0]);
        break;
    case "success_login":
        FinalizeLogin();
        break;
    case "success_signup":
        loginBrowser.call("SetLoginData");
        // TODO#41 - Display popup that signup was successful, now login
        API.sendChatMessage("~g~Registration successful, now you can login.");
        break;
    }
});

API.onUpdate.connect(function ()
{
    if (loginBrowser == null) return;
    API.disableAllControlsThisFrame();
});

function InitializeLogin() {
    CreateBrowser();
    API.showCursor(true);
    API.setCanOpenChat(false);
}

function FinalizeLogin() {
    API.destroyCefBrowser(loginBrowser);
    API.showCursor(false);
    API.setCanOpenChat(true);
    API.setActiveCamera(null);
    Camera = null;
    loginBrowser = null;
}

function OnDisplayLoginEvent() {
    InitializeLogin();
    API.sleep(1000);
    loginBrowser.call("SetLoginData");
    loginBrowser.call("SetUsername", API.getPlayerName(API.getLocalPlayer()));
    API.setCefBrowserHeadless(loginBrowser, false);
}

function OnDisplaySignupEvent() {
    InitializeLogin();
    API.sleep(1000);
    loginBrowser.call("SetSignupData");
    loginBrowser.call("SetUsername", API.getPlayerName(API.getLocalPlayer()));
    API.setCefBrowserHeadless(loginBrowser, false);
}

function CreateBrowser() {
    if (loginBrowser != null) return;
    var res = API.getScreenResolution();
    var width = 750;
    var height = 750;
    var xPos = (res.Width / 2.0) - (width / 2.0);
    var yPos = (res.Height / 2.0) - (height / 2.0);
    loginBrowser = API.createCefBrowser(width, height);
    API.waitUntilCefBrowserInit(loginBrowser);
    API.setCefBrowserPosition(loginBrowser, xPos, yPos);
    API.setCefBrowserHeadless(loginBrowser, true);
    API.loadPageCefBrowser(loginBrowser, "Client/CEF/login.html");
}

function LoginPlayer(password) {
    API.triggerServerEvent("player_login", password);
}

function SignupPlayer(password, passwordConfirm) {
    if(password !== passwordConfirm) {
        loginBrowser.call("EnableSubmitButton", true);
        // TODO#41 - Display popup that the passwords do not match
        API.sendChatMessage("~r~ERROR:~w~ Registration failed because your passwords do not match");
        return;
    }
    API.triggerServerEvent("player_signup", password);
}