var currentMoney = null;
var res_X = API.getScreenResolutionMantainRatio().Width;

let menuPool = null;

API.onKeyDown.connect(function (Player, args) {
    /*if (args.KeyCode == Keys.E && !API.isChatOpen()) {
        menuPool = API.getMenuPool();
        let menu = API.createMenu("Test", 0, 0, 6);
        let item1 = API.createMenuItem("Item1", "Description1");
        let item2 = API.createMenuItem("Item2", "Description1");
        let item3 = API.createMenuItem("Item3", "Description1");
        menu.AddItem(item1);
        menu.AddItem(item2);
        menu.AddItem(item3);
        menuPool.Add(menu);
        menu.Visible = true;

        menu.OnItemSelect.connect(function (sender, item, index) {
            API.sendChatMessage("You selected: ~g~" + item.Text);

            menu.Visible = false;
        });
    }*/
});

API.onServerEventTrigger.connect(function (name, args) {
    if (name === "update_money_display") {
        currentMoney = args[0];
    }
});

API.onUpdate.connect(function() {
    if (currentMoney != null) {
        API.drawText("$" + currentMoney, res_X - 15, 50, 1, 115, 186, 131, 255, 4, 2, false, true, 0);
    }
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});