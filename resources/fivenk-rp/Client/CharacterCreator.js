var CharacterBrowser = null;
var Camera = null;
var CharacterCameraPos = new Vector3(3513.92, 5118.72, 5.76);
var CharacterRotation = new Vector3(0, 0, 235.89);
var CharString = "[]";
var HasCharacters = false;

var CharacterCreatorPath = "Client/CEF/charactercreator.html";
var CharacterSelectorPath = "Client/CEF/characterselector.html";

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
    case "display_character_selector":
        if(args.length == 0) return;
        CharString = args[0];
        API.showCursor(true);
        API.setCanOpenChat(false);
        OnDisplayCharacterSelector();
        break;
    case "display_character_creator":
        API.showCursor(true);
        API.setCanOpenChat(false);
        OnDisplayCharacterCreator();
        break;
    case "fail_create_character":
        // TODO#41 - Display popup that creation failed args[0] is reason
        API.sendChatMessage("~r~ERROR:~w~ Character creation failed: " + args[0]);
        CharacterBrowser.call("EnableButtons", true);
        break;
    case "success_create_character":
        ReturnToCharacterSelector();
        break;
    case "fail_select_character":
        // TODO#41 - Display popup that creation failed args[0] is reason
        API.sendChatMessage("~r~ERROR:~w~ Character selection failed: " + args[0]);
        CharacterBrowser.call("EnableButtons", true);
        break;
    case "success_select_character":
        OnEnterGame();
        break;
    }
});

API.onUpdate.connect(function ()
{
    if (CharacterBrowser == null && Camera == null)
    {
        // Disable controls while the browser or custom camera are active
        return;
    }
    API.disableAllControlsThisFrame();
});

function CreateCreatorBrowser() {
    if (CharacterBrowser != null) return;
    // Position in terms of 2560x1440 so normalize:
    var res = API.getScreenResolution();
    var xPos = 1520 * (res.Width / 2560.0);
    var yPos = 450 * (res.Height / 1440.0);
    var width = res.Width - xPos;
    var height = res.Height - yPos;
    CharacterBrowser = API.createCefBrowser(width, height);
    API.waitUntilCefBrowserInit(CharacterBrowser);
    API.setCefBrowserPosition(CharacterBrowser, xPos, yPos);
    API.setCefBrowserHeadless(CharacterBrowser, true);
    API.loadPageCefBrowser(CharacterBrowser, CharacterCreatorPath);
}

function CreateSelectorBrowser() {
    if (CharacterBrowser != null) return;
    // Position in terms of 2560x1440 so normalize:
    var res = API.getScreenResolution();
    var xPos = 950 * (res.Width / 2560.0);
    var yPos = 400 * (res.Height / 1440.0);
    var width = 1300 * (res.Width / 2560.0);
    var height = 1000 * (res.Height / 1440.0);
    CharacterBrowser = API.createCefBrowser(width, height);
    API.waitUntilCefBrowserInit(CharacterBrowser);
    API.setCefBrowserPosition(CharacterBrowser, xPos, yPos);
    API.setCefBrowserHeadless(CharacterBrowser, true);
    API.loadPageCefBrowser(CharacterBrowser, CharacterSelectorPath);
}

function DestroyBrowser() {
    API.destroyCefBrowser(CharacterBrowser);
    CharacterBrowser = null;
}

function SetPlayerSkin(SkinHash) {
    API.setPlayerSkin(SkinHash);
    // Make sure character faces camera
    API.setEntityRotation(API.getLocalPlayer(), CharacterRotation);
}

function ShowCharacterCreator() {
    API.setCefBrowserHeadless(CharacterBrowser, false);
    CharacterBrowser.call("ShouldShowReturn", HasCharacters);
}

function ShowCharacterSelector() {
    API.setCefBrowserHeadless(CharacterBrowser, false);
    CharacterBrowser.call("SetCharacters", CharString);
}

function SetupCamera() {
    if(Camera != null) return;
    Camera = API.createCamera(CharacterCameraPos, new Vector3());
    API.pointCameraAtEntity(Camera, API.getLocalPlayer(), new Vector3());
    API.setActiveCamera(Camera);
    API.setEntityRotation(API.getLocalPlayer(), CharacterRotation);
}

function DestroyCamera() {
    API.setActiveCamera(null);
    Camera = null;
}

function OnDisplayCharacterSelector() {
    SetupCamera();
    CreateSelectorBrowser();
    HasCharacters = true;
}

function OnDisplayCharacterCreator() {
    SetupCamera();
    CreateCreatorBrowser();
}

function OnSelectCreateCharactor() {
    DestroyBrowser();
    CreateCreatorBrowser();
}

function OnSelectCharacter(CharacterId) {
    API.triggerServerEvent("select_character", CharacterId);
}

function OnEnterGame() {
    DestroyCamera();
    DestroyBrowser();
    API.showCursor(false);
    API.setCanOpenChat(true);
}

function CreateCharacter(CharacterName, GroupIndex, SkinHash) {
    API.triggerServerEvent("create_character", CharacterName, GroupIndex, SkinHash);
}

function ReturnToCharacterSelector() {
    ExitCharacterCreator();
    API.triggerServerEvent("display_character_selector");
}

function ExitCharacterCreator() {
    DestroyBrowser();
}