var CharacterBrowser = null;
var Camera = null;
var CharacterCameraPos = new Vector3(3513.92, 5118.72, 5.76);

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
    case "display_character_selector":
        if(args.length == 0) return;
        OnDisplayCharacterSelector(args[0]);
        break;
    case "display_character_creator":
        OnDisplayCharacterCreator();
        break;
    }
});

API.onUpdate.connect(function ()
{
    if (CharacterBrowser == null) return;
    API.disableAllControlsThisFrame();
});

function CreateBrowser() {
    if (CharacterBrowser != null) return;
    // Position in terms of 2560x1440 so normalize:
    var res = API.getScreenResolution();
    var xPos = 1340 * (res.Width / 2560.0);
    var yPos = 450 * (res.Height / 1440.0);
    var width = res.Width - xPos;
    var height = res.Height - yPos;
    CharacterBrowser = API.createCefBrowser(width, height);
    API.waitUntilCefBrowserInit(CharacterBrowser);
    API.setCefBrowserPosition(CharacterBrowser, xPos, yPos);
    API.setCefBrowserHeadless(CharacterBrowser, false);
    API.loadPageCefBrowser(CharacterBrowser, "Client/CEF/charactercreator.html");
    API.showCursor(true);
    API.setCanOpenChat(false);
}

function SetupCamera() {
    Camera = API.createCamera(CharacterCameraPos, new Vector3());
    API.pointCameraAtEntity(Camera, API.getLocalPlayer(), new Vector3());
    API.setActiveCamera(Camera);
}

function OnDisplayCharacterSelector(characters) {
    if(characters.length == 0) {
        OnDisplayCharacterCreator();
        return;
    }
}

function OnDisplayCharacterCreator() {
    SetupCamera();
    CreateBrowser();
}