var CharacterBrowser = null;
var Camera = null;
var CharacterCameraPos = new Vector3(3513.92, 5118.72, 5.76);
var DoesHaveCharacters = false;

var CharacterCreatorPath = "Client/CEF/charactercreator.html";
var CharacterSelectorPath = "Client/CEF/characterselector.html";

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
    case "display_character_selector":
        if(args.length == 0) return;
        OnDisplayCharacterSelector(args[0]);
        break;
    case "display_character_creator":
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
    }
});

API.onUpdate.connect(function ()
{
    if (CharacterBrowser == null) return;
    API.disableAllControlsThisFrame();
});

function CreateBrowser(PagePath) {
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
    API.loadPageCefBrowser(CharacterBrowser, PagePath);
    API.showCursor(true);
    API.setCanOpenChat(false);
}

function SetPlayerSkin(SkinHash) {
    API.setPlayerSkin(SkinHash);
}

function ShowCharacterCreator() {
    API.setCefBrowserHeadless(CharacterBrowser, false);
    CharacterBrowser.call("ShouldShowReturn", DoesHaveCharacters);
}

function ShowCharacterSelector() {
    API.setCefBrowserHeadless(CharacterBrowser, false);
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
    DoesHaveCharacters = true;
    SetupCamera();
    CreateBrowser(CharacterSelectorPath);
}

function OnDisplayCharacterCreator() {
    SetupCamera();
    CreateBrowser(CharacterCreatorPath);
}

function CreateCharacter(CharacterName, GroupIndex, SkinHash) {
    API.triggerServerEvent("create_character", CharacterName, GroupIndex, SkinHash);
}

function ReturnToCharacterSelector() {
    ExitCharacterCreator();
    // TODO reopen character selector
    API.setActiveCamera(null);
    Camera = null;
}

function ExitCharacterCreator() {
    API.destroyCefBrowser(CharacterBrowser);
    API.showCursor(false);
    API.setCanOpenChat(true);
    CharacterBrowser = null;
}